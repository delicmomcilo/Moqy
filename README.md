# Moqy

Moqy is an open-source .NET Core Web API that allows users to mock the behavior of a streaming API by defining a schema. It's particularly useful for development scenarios where you want to simulate responses from APIs without incurring costs or dependencies on external services.

## Features 🚀

- 🎭 Mock streaming API responses based on a provided schema
- 🔧 Customizable number of items and delay between items
- 🏗️ Supports nested objects and arrays in the schema
- 🤖 Uses Faker.NET for generating realistic mock data
- 💻 Easy to integrate and use in development environments
- 📝 Supports simple JSON format for quick mocking
- ⏹️ Stream abortion support for client control over streaming

## Getting Started 🛠️

1. Clone the repository
2. Navigate to the project directory
3. Build and run the project using Docker:

    ```bash
    docker-compose up --build
    ```

This will start the Moqy API. The API will be available at `http://localhost:8080`.

## Usage 📡

### Streaming API

Send a POST request to `/api/mock/stream` with the following:

#### Body

- JSON schema defining the structure of the mock data, or
- Simple JSON format

#### Query Parameters

- `isSimpleJson`: Boolean flag to indicate if the body is a simple JSON format (default: `false`)
- `delayMs`: Delay between items in milliseconds (default: `2000`)

### Example Requests 📋

#### Detailed Schema Example

```json
{
  "type": "object",
  "properties": {
    "id": { 
      "type": "string", 
      "format": "uuid" 
    },
    "createdAt": { 
      "type": "string", 
      "format": "date-time" 
    },
    "user": {
      "type": "object",
      "properties": {
        "name": { 
          "type": "string"
        },
        "email": { 
          "type": "string", 
          "format": "email" 
        },
        "age": {
          "type": "integer",
          "minimum": 18,
          "maximum": 100
        }
      }
    },
    "tags": {
      "type": "array",
      "items": { "type": "string" },
      "minItems": 1,
      "maxItems": 5
    },
    "status": {
      "type": "string",
      "enum": ["pending", "approved", "rejected"]
    }
  }
}
```

#### Simple JSON Example

**Request URL:** `http://localhost:8080/api/mock/stream?isSimpleJson=true&delayMs=500`

**Request Body:**
```json
{
  "id": "string",
  "name": "string",
  "age": 25,
  "tags": ["string1", "string2"]
}
```

### Stream Control 🕹️

The client can start and stop the streaming process using the `AbortController` on the client-side.

## Contributing 🤝

Contributions are welcome! Please feel free to submit a Pull Request.

## License 📜

This project is licensed under the MIT License.
