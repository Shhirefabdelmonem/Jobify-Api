# Recommendation Service

## Overview

The RecommendationService provides personalized job recommendations to job seekers by integrating with a FastAPI-based AI model. This service follows Clean Architecture principles and implements CQRS patterns with comprehensive error handling, retry logic, and performance monitoring.

## Architecture

### Clean Architecture Layers

1. **Application Layer** (`Jobify.Services`)

   - `GetRecommendationsQuery` - Query object for requesting recommendations
   - `GetRecommendationsQueryHandler` - Handles the business logic
   - `GetRecommendationsQueryValidator` - Validates input parameters

2. **Infrastructure Layer** (`Jobify.Infrastructure`)

   - `RecommendationService` - HTTP client implementation for FastAPI communication
   - Service registration and configuration

3. **API Layer** (`Jobify.Api`)
   - `RecommendationController` - REST API endpoints

## Features

### CQRS Implementation

- **Query**: `GetRecommendationsQuery` with validation
- **Handler**: `GetRecommendationsQueryHandler` with comprehensive error handling
- **Validator**: Input validation using FluentValidation

### MediatR Pipeline Behaviors

- **Logging**: Request/response logging using Serilog
- **Validation**: Automatic input validation
- **Performance Monitoring**: Tracks execution time and identifies slow requests
- **Retry Logic**: Automatic retry for transient failures (HTTP errors, timeouts)

### HTTP Client Features

- **Typed HttpClient**: Configured with HttpClientFactory
- **Timeout Configuration**: Configurable request timeouts
- **Error Handling**: Graceful handling of HTTP errors, timeouts, and JSON parsing errors
- **Logging**: Comprehensive logging for debugging and monitoring

## Configuration

Add the following configuration to `appsettings.json`:

```json
{
  "Recommendation": {
    "BaseUrl": "http://localhost:8000",
    "RecommendationEndpoint": "/recommend",
    "TimeoutSeconds": 30,
    "MaxRetryAttempts": 3,
    "RetryDelaySeconds": 2
  }
}
```

## API Usage

### Endpoint

```
POST /api/recommendation/get-recommendations
```

### Request Body

```json
{
  "name": "John Doe",
  "degree": "Bachelor's",
  "major": "Computer Science",
  "gpa": 3.5,
  "experience": 2,
  "skills": "C#, .NET, SQL, JavaScript"
}
```

### Response

```json
{
  "success": true,
  "message": "Recommendations retrieved successfully",
  "statusCode": 200,
  "data": {
    "recommended_jobs": [
      {
        "job_title": "Fullstack Developer",
        "company": "Syad Tech",
        "skills_required": "C#; .NET; SQL; JavaScript",
        "combined_match_score": "80.66%",
        "semantic_score": "61.32%",
        "keyword_score": "100.0%",
        "job_link": "https://example.com"
      }
    ]
  }
}
```

## Error Handling

The service handles various error scenarios:

- **HTTP Errors**: Network connectivity issues, service unavailability
- **Timeouts**: Request timeouts with configurable thresholds
- **JSON Parsing**: Invalid response format handling
- **Validation**: Input parameter validation with detailed error messages

## Monitoring and Logging

### Performance Monitoring

- Tracks request execution time
- Identifies slow requests (>5 seconds)
- Logs performance metrics

### Logging Levels

- **Information**: Successful requests and performance metrics
- **Warning**: Slow requests and retry attempts
- **Error**: Failed requests and exceptions

### Retry Logic

- Automatic retry for transient failures
- Exponential backoff strategy
- Configurable retry attempts and delays

## Testing

The service is designed to be easily testable:

1. **Unit Tests**: Mock `IRecommendationService` for handler testing
2. **Integration Tests**: Use TestServer for end-to-end testing
3. **HTTP Client Tests**: Mock HttpClient responses

## Extensibility

The service is designed for easy extension:

1. **New Recommendation Models**: Add new query types and handlers
2. **Additional Business Logic**: Extend handlers with caching, filtering, etc.
3. **Multiple AI Services**: Implement additional recommendation providers
4. **Enhanced Monitoring**: Add custom metrics and health checks

## Dependencies

- **MediatR**: CQRS implementation
- **FluentValidation**: Input validation
- **Microsoft.Extensions.Http**: HTTP client factory
- **Serilog**: Structured logging
- **System.Text.Json**: JSON serialization

## Security

- **Authorization**: Requires valid JWT token
- **Input Validation**: Comprehensive parameter validation
- **Error Information**: Sanitized error messages to prevent information disclosure
