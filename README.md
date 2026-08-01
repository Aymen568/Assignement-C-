# Machine Heartbeat Service

A real-time machine monitoring system built with ASP.NET Core 8, featuring REST API, gRPC heartbeat ingestion, and SignalR dashboard updates.

## Prerequisites

- **.NET 8 SDK** – Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

## Running the Application

```bash
dotnet run
```

The application listens on:
- **HTTP**: `http://localhost:5156`
- **HTTPS**: `https://localhost:7250`

(Configured in `Properties/launchSettings.json`)

**Swagger API Documentation** is available at:
- `https://localhost:7250/swagger` (in Development mode)

## REST Endpoints

### Create a Machine

```bash
curl -X POST https://localhost:7250/api/machines \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Machine-001",
    "metadata": {"location": "warehouse-a", "owner": "team-ops"}
  }'
```

**Response (201 Created):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Machine-001",
  "status": "Offline",
  "lastHeartbeat": "0001-01-01T00:00:00+00:00",
  "currentJob": "",
  "metadata": {"location": "warehouse-a", "owner": "team-ops"}
}
```

### Get All Machines

```bash
curl https://localhost:7250/api/machines
```

### Get a Specific Machine

```bash
curl https://localhost:7250/api/machines/{id}
```
Replace `{id}` with a valid machine GUID.

### Delete a Machine

```bash
curl -X DELETE https://localhost:7250/api/machines/{id}
```

### Get Metrics/Health

```bash
curl https://localhost:7250/api/metrics
```

**Response:**
```json
{
  "totalMachines": 5,
  "onlineMachines": 3,
  "offlineMachines": 2
}
```

## gRPC Heartbeat Endpoint

Heartbeats are sent via **gRPC**, not REST. Use a gRPC client such as:
- **[grpcurl](https://github.com/fullstorydev/grpcurl)** – command-line tool
- **[BloomRPC](https://www.bloomrpc.com/)** – GUI client
- Any gRPC library in your preferred language

### Send a Heartbeat with grpcurl

```bash
grpcurl -plaintext \
  -d '{
    "machine_id": "550e8400-e29b-41d4-a716-446655440000",
    "timestamp": "'$(date -u +%Y-%m-%dT%H:%M:%SZ)'",
    "cpu_usage": 45.5,
    "memory_usage": 60.2,
    "temperature": 72.1,
    "current_job": "backup-data"
  }' \
  localhost:5156 assignement.HeartbeatService/SendHeartbeat
```

**Note**: 
- `machine_id` must be a valid GUID and must correspond to an existing machine.
- `timestamp` is in RFC 3339 format (ISO 8601).
- Heartbeats are idempotent: duplicate or stale timestamps are suppressed and generate no events.

## SignalR Dashboard Hub

Connect to the dashboard hub to receive real-time machine status updates:

**Hub URL**: `wss://localhost:7250/hubs/dashboard` (over HTTPS/WSS)  
Or: `ws://localhost:5156/hubs/dashboard` (over HTTP/WS)

Example with JavaScript:

```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:7250/hubs/dashboard")
  .withAutomaticReconnect()
  .build();

connection.on("MachineStatusChanged", (machineId, oldStatus, newStatus) => {
  console.log(`Machine ${machineId}: ${oldStatus} → ${newStatus}`);
});

connection.on("HeartbeatProcessed", (machineId, timestamp, currentJob, metrics) => {
  console.log(`Heartbeat from ${machineId}: job=${currentJob}, cpu=${metrics.cpuUsage}%`);
});

connection.start();
```

## Assumptions

- **Status is modeled as an enum**: `Online` or `Offline` (not a boolean).
- **Machine creation does not require unique ID generation by the client**: the service auto-generates a GUID.
- **Machine creation validates name uniqueness** (case-insensitive) to prevent duplicates.
- **Offline detection runs every ~5 seconds** against a **30-second heartbeat timeout** (configurable via `OfflineDetectionService`).
- **Heartbeat timestamps are in UTC** and must be in RFC 3339/ISO 8601 format.
- **Duplicate or stale heartbeats** (with a timestamp ≤ the last recorded heartbeat) are silently dropped and generate no events.
- **In-memory storage**: machines are persisted in memory and reset on application restart.
- **Request/response metadata** is passed via gRPC protobuf messages; no custom serialization is required.

## Implementation Notes

- **Concurrency**: The heartbeat processor uses Compare-And-Swap (CAS) with retry logic (max 3 retries) to safely handle concurrent updates. Retries are bounded to prevent resource exhaustion.
- **Idempotency**: Duplicate heartbeat events are suppressed by comparing incoming timestamps with the machine's `LastHeartbeat` field.
- **Data Validation**: 
  - The gRPC service validates `machine_id` as a valid, non-empty GUID before processing.
  - The REST API validates `CreateMachineRequest.Name` with `[Required]` and `[MinLength(1)]` attributes (auto-enforced by `[ApiController]`).
  - The service checks for duplicate machine names (case-insensitive) on creation.
- **Status Enum**: Machine status is now strongly-typed as `MachineStatus` (Offline/Online) instead of bool for clarity and type safety.
- **Event Publishing**: Machine creation, deletion, heartbeat processing, and status changes all publish events to subscribed SignalR clients.
- **Health Metrics**: The `/api/metrics` endpoint provides a quick health check with machine counts.
- **API Documentation**: Swagger/OpenAPI is enabled in Development mode at `/swagger` for easy endpoint exploration.
