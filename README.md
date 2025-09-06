## Description
This project follows the principles of Clean Architecture in ASP.NET Core, ensuring a modular, maintainable, and testable codebase. The architecture separates concerns into distinct layers (Domain, Application, Infrastructure, and Presentation), making the system easier to extend and scale.

## Features
- Clean Architecture principles applied for maintainability and testability.
- Swagger/OpenAPI for API documentation and testing.
- Docker Compose setup for containerized development and deployment.
- Separation of concerns with distinct layers for domain logic, application services, and infrastructure.

## Project Structure
- `Domain`: Contains the core business logic and entities.
- `Application`: Contains application services, interfaces, and DTOs.
- `Infrastructure`: Contains data access implementations and external service integrations.
- `WebApi`: Contains the ASP.NET Core Web API project.

## Running with Docker Compose

To run the project using Docker Compose, ensure you have Docker and Docker Compose installed on your system.

1.  Make sure you are in the root directory of the project where the `compose.yaml` file is located.
2.  Run the following command to build the images and start the containers:
    ```shell
    docker-compose up --build
    ```
    You can add the `-d` flag to run the containers in detached mode (in the background):
    ```shell
    docker-compose up --build -d
    ```

3.  Once the application is running, you can find the API documentation at `https://localhost:8001/swagger/index.html`.
4.  To stop and remove the containers, networks, and volumes, run:
    ```shell
    docker-compose down
    ```

## API Endpoints

### UsersController
- **POST /users/register**
  - Registers a new user.
  - Request body: `{ username, email, password }`
  - Response: Registration result (success or error).

- **POST /users/login**
  - Authenticates a user (login).
  - Request body: `{ identifier, password }`
  - Response: Authentication result (token or error).

### TasksController
- **POST /tasks**
  - Creates a new task for the authenticated user.
  - Request body: `{ title, description, dueDate, status, priority }`
  - Requires authentication.
  - Response: Task creation result.

- **GET /tasks**
  - Retrieves a paginated list of tasks for the authenticated user.
  - Query parameters: `pageNumber`, `pageSize`, `status`, `dueDate`, `priority`
  - Requires authentication.
  - Response: List of tasks.

- **GET /tasks/{id}**
  - Retrieves a specific task by its ID for the authenticated user.
  - Requires authentication.
  - Response: Task details.

- **PUT /tasks/{id}**
  - Updates a specific task by its ID for the authenticated user.
  - Request body: `{ title, description, dueDate, status, priority }`
  - Requires authentication.
  - Response: Update result.

- **DELETE /tasks/{id}**
  - Deletes a specific task by its ID for the authenticated user.
  - Requires authentication.
  - Response: Deletion result.

## Seed Users and Data

The application is seeded with several test users and a variety of tasks for development and testing purposes. You can use these accounts to log in and interact with the API.

### Available Users
| Username   | Email                  | Password      |
|------------|------------------------|---------------|
| johndoe    | johndoe@example.com    | Password123!  |
| janesmith  | janesmith@example.com  | Password123!  |
| alice      | alice@example.com      | Password123!  |
| bob        | bob@example.com        | Password123!  |
| charlie    | charlie@example.com    | Password123!  |

All users have the same password: `Password123!`

### Seeded Tasks
- Each user is assigned a set of tasks relevant to their role (e.g., backend, frontend, management, DevOps, QA).
- Tasks cover a range of statuses (`Completed`, `InProgress`) and priorities (`High`, `Medium`, `Low`).
- Example task fields: `title`, `description`, `status`, `priority`, `dueDate` (format: ISO 8601, e.g. `2025-09-06T12:00:00Z`).

### Authentication
- All API endpoints requiring authentication expect a JWT token in the `Authorization` header: `Bearer <token>`.
- Use the above credentials to obtain a token via the `/users/login` endpoint.

### Seed Logic
- The database is seeded automatically if empty, creating the above users and a variety of tasks for each.
- See `SeedData.cs` for details on the seed logic and task assignment.
