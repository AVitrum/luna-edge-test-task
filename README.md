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