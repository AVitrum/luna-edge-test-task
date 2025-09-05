using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Interfaces;
using MyProject.Domain.Entities;
using MyProject.Domain.Enums;
using Task = System.Threading.Tasks.Task;
using TaskStatus = MyProject.Domain.Enums.TaskStatus;

namespace MyProject.Infrastructure.Persistence.Seeds;

/// <summary>
/// Provides functionality to seed the database with initial data for development and testing.
/// </summary>
public static class SeedData
{
    /// <summary>
    /// Initializes the database with a predefined set of users and tasks if the database is empty.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve required services like the DbContext and password hasher.</param>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await using var context = serviceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();

        // Check if users already exist to prevent re-seeding.
        if (await context.Users.AnyAsync())
        {
            return;
        }

        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var user3Id = Guid.NewGuid();
        var user4Id = Guid.NewGuid();
        var user5Id = Guid.NewGuid();

        var users = new[]
        {
            new User
            {
                Id = user1Id,
                Username = "johndoe",
                Email = "johndoe@example.com",
                PasswordHash = passwordHasher.Hash("Password123!")
            },
            new User
            {
                Id = user2Id,
                Username = "janesmith",
                Email = "janesmith@example.com",
                PasswordHash = passwordHasher.Hash("Password123!")
            },
            new User
            {
                Id = user3Id,
                Username = "alice",
                Email = "alice@example.com",
                PasswordHash = passwordHasher.Hash("Password123!")
            },
            new User
            {
                Id = user4Id,
                Username = "bob",
                Email = "bob@example.com",
                PasswordHash = passwordHasher.Hash("Password123!")
            },
            new User
            {
                Id = user5Id,
                Username = "charlie",
                Email = "charlie@example.com",
                PasswordHash = passwordHasher.Hash("Password123!")
            }
        };

        await context.Users.AddRangeAsync(users);

        var tasks = new List<Domain.Entities.Task>
        {
            // Tasks for John Doe (Backend Developer)
            new() { Title = "Setup project repository", Description = "Initialize git and push to remote.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Develop authentication feature", Description = "Implement user registration and login.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(5) },
            new() { Title = "Review code pull request #42", Description = "Check the latest changes from the feature branch.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(1) },
            new() { Title = "Implement password reset", Description = "Add endpoint for password reset requests.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(8) },
            new() { Title = "Design API for user profiles", Description = "Define CRUD endpoints for user profiles.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(12) },
            new() { Title = "Add logging middleware", Description = "Log all incoming HTTP requests.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(-3) },
            new() { Title = "Optimize database queries", Description = "Analyze and improve slow queries on the dashboard.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(15) },
            new() { Title = "Integrate with payment gateway", Description = "Implement Stripe for subscription payments.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(20) },
            new() { Title = "Create email sending background job", Description = "Use Hangfire for sending welcome emails.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(10) },
            new() { Title = "Write integration tests for Auth", Description = "Cover login, registration, and token refresh.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(18) },
            new() { Title = "Set up global exception handling", Description = "Create a middleware to catch unhandled exceptions.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(-10) },
            new() { Title = "Implement JWT refresh token", Description = "Allow users to refresh expired access tokens.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(25) },
            new() { Title = "Add rate limiting to public APIs", Description = "Prevent abuse of public endpoints.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(14) },
            new() { Title = "Configure CORS policy", Description = "Allow requests from the frontend application domain.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(-20) },
            new() { Title = "Develop file upload feature", Description = "Allow users to upload profile pictures.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(30) },
            new() { Title = "Create data migration script", Description = "Migrate data from old schema to new schema.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(40) },
            new() { Title = "Refactor notification service", Description = "Improve performance and readability.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(35) },
            new() { Title = "Secure API endpoints", Description = "Apply [Authorize] attribute where needed.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(9) },
            new() { Title = "Document 'orders' API", Description = "Add Swagger documentation for the new endpoint.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(22) },
            new() { Title = "Investigate search performance", Description = "Product search is slow with many items.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user1Id, DueDate = DateTime.UtcNow.AddDays(13) },

            // Tasks for Jane Smith (Frontend Developer)
            new() { Title = "Design database schema", Description = "Create ERD for the main entities.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Write API documentation", Description = "Use Swagger/OpenAPI for documentation.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(10) },
            new() { Title = "Implement caching strategy", Description = "Use Redis for caching frequent queries.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(7) },
            new() { Title = "Develop user profile page", Description = "Build the UI for viewing and editing user profiles.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(4) },
            new() { Title = "Create reusable button component", Description = "Component should support different styles and sizes.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(-5) },
            new() { Title = "Implement state management", Description = "Use Redux Toolkit for global state.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(15) },
            new() { Title = "Style application for responsiveness", Description = "Ensure layout works on mobile, tablet, and desktop.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(11) },
            new() { Title = "Write unit tests for login form", Description = "Use Jest and React Testing Library.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(6) },
            new() { Title = "Integrate with user API", Description = "Connect frontend to backend for user data.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(8) },
            new() { Title = "Set up frontend build process", Description = "Configure Vite for development and production builds.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(-20) },
            new() { Title = "Create theme switcher", Description = "Implement light and dark mode toggle.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(25) },
            new() { Title = "Implement form validation", Description = "Add client-side validation to the registration form.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new() { Title = "Add internationalization (i18n)", Description = "Support English and Spanish languages.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(30) },
            new() { Title = "Optimize frontend asset loading", Description = "Implement code splitting and lazy loading.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(18) },
            new() { Title = "Make application accessible", Description = "Ensure compliance with WCAG 2.1 AA standards.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(40) },
            new() { Title = "Create Storybook for components", Description = "Document all reusable UI components.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(35) },
            new() { Title = "Fix navigation bar CSS bug", Description = "The logo is misaligned on Safari.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(1) },
            new() { Title = "Implement optimistic UI updates", Description = "Improve perceived performance for task creation.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(22) },
            new() { Title = "Connect to WebSocket", Description = "Listen for real-time notifications from the server.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(28) },
            new() { Title = "Build the settings page UI", Description = "Create the UI for user account settings.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user2Id, DueDate = DateTime.UtcNow.AddDays(13) },

            // Tasks for Alice (Project Manager)
            new() { Title = "Create UI mockups for dashboard", Description = "Design the main dashboard view.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new() { Title = "Prepare presentation for stakeholders", Description = "Summarize project progress.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(6) },
            new() { Title = "Fix bug #101 - Login page style", Description = "The submit button is misaligned on mobile.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Define Q3 project roadmap", Description = "Plan features and milestones for the next quarter.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(20) },
            new() { Title = "Gather user feedback on new feature", Description = "Conduct user interviews for the beta feature.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(15) },
            new() { Title = "Update project management board", Description = "Move tasks in Jira to reflect current status.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Schedule weekly sync meeting", Description = "Coordinate with the development team.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Write user stories for payment feature", Description = "Detail requirements for subscription flow.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(5) },
            new() { Title = "Analyze competitor products", Description = "Identify strengths and weaknesses of competitors.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(30) },
            new() { Title = "Create project budget report", Description = "Track expenses against the allocated budget.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(10) },
            new() { Title = "Onboard new designer", Description = "Introduce the project and current design system.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(4) },
            new() { Title = "Prioritize backlog tasks", Description = "Work with product owner to order the backlog.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(2) },
            new() { Title = "Draft release notes for v1.2", Description = "Summarize new features and bug fixes.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(12) },
            new() { Title = "Review UI/UX designs", Description = "Provide feedback on the new profile page mockups.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(7) },
            new() { Title = "Plan user acceptance testing (UAT)", Description = "Define test cases and schedule UAT sessions.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(25) },
            new() { Title = "Resolve team conflicts", Description = "Mediate a disagreement between two developers.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(1) },
            new() { Title = "Report project status to management", Description = "Prepare a weekly status report.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(6) },
            new() { Title = "Define success metrics for launch", Description = "Establish KPIs to measure feature success.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(18) },
            new() { Title = "Organize a project retrospective", Description = "Discuss what went well and what could be improved.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(32) },
            new() { Title = "Approve vacation requests", Description = "Manage team availability and schedule.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user3Id, DueDate = DateTime.UtcNow.AddDays(-3) },

            // Tasks for Bob (DevOps Engineer)
            new() { Title = "Set up CI/CD pipeline", Description = "Configure GitHub Actions for automated builds.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(-5) },
            new() { Title = "Write unit tests for TaskService", Description = "Ensure 90% code coverage.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(4) },
            new() { Title = "Refactor legacy code in PaymentModule", Description = "Improve readability and performance.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(14) },
            new() { Title = "Configure production environment", Description = "Set up servers and databases on AWS.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(8) },
            new() { Title = "Implement infrastructure as code", Description = "Use Terraform to manage cloud resources.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(20) },
            new() { Title = "Set up monitoring and alerting", Description = "Use Prometheus and Grafana for application monitoring.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(12) },
            new() { Title = "Automate database backups", Description = "Schedule daily backups of the production database.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(-10) },
            new() { Title = "Perform security audit", Description = "Scan for vulnerabilities in the application and infrastructure.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(30) },
            new() { Title = "Manage SSL certificates", Description = "Renew SSL certificates for all domains.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(5) },
            new() { Title = "Optimize container images", Description = "Reduce the size of Docker images for faster deployments.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(16) },
            new() { Title = "Create a disaster recovery plan", Description = "Document steps to recover from a major outage.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(45) },
            new() { Title = "Configure a VPN for secure access", Description = "Provide secure access to internal resources.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(-30) },
            new() { Title = "Manage user access and permissions", Description = "Set up IAM roles and policies in AWS.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new() { Title = "Troubleshoot deployment failure", Description = "Investigate why the last deployment to staging failed.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(1) },
            new() { Title = "Set up a centralized logging system", Description = "Aggregate logs from all services into Elasticsearch.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(25) },
            new() { Title = "Automate dependency updates", Description = "Use Dependabot to keep packages up to date.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(18) },
            new() { Title = "Conduct load testing", Description = "Simulate high traffic to identify performance bottlenecks.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(40) },
            new() { Title = "Document CI/CD process", Description = "Explain how code gets from commit to production.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(28) },
            new() { Title = "Upgrade Kubernetes cluster", Description = "Upgrade the EKS cluster to the latest version.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(50) },
            new() { Title = "Review infrastructure costs", Description = "Identify opportunities to reduce cloud spending.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user4Id, DueDate = DateTime.UtcNow.AddDays(22) },

            // Tasks for Charlie (QA Engineer)
            new() { Title = "Deploy staging environment", Description = "Push the latest build to the staging server.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(-1) },
            new() { Title = "Research third-party email providers", Description = "Compare SendGrid, Mailgun, and AWS SES.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(8) },
            new() { Title = "Organize team-building event", Description = "Plan an activity for the end of the quarter.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(20) },
            new() { Title = "Write test plan for authentication", Description = "Define test cases for the login and registration flows.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(3) },
            new() { Title = "Perform regression testing for v1.2", Description = "Test existing functionality to prevent regressions.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(10) },
            new() { Title = "Automate UI tests for login page", Description = "Use Cypress to create end-to-end tests.", Status = TaskStatus.InProgress, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(15) },
            new() { Title = "Test API endpoints with Postman", Description = "Verify responses for the user profile API.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(5) },
            new() { Title = "Report bug in password reset", Description = "The reset link expires immediately.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Verify mobile responsiveness", Description = "Check the application on various mobile devices.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(7) },
            new() { Title = "Conduct exploratory testing", Description = "Freely explore the application to find bugs.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(12) },
            new() { Title = "Test for accessibility issues", Description = "Use screen readers and other tools to test accessibility.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(25) },
            new() { Title = "Set up a test data generator", Description = "Create a script to populate the test database.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(30) },
            new() { Title = "Perform cross-browser testing", Description = "Test the application on Chrome, Firefox, and Safari.", Status = TaskStatus.Completed, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(9) },
            new() { Title = "Validate analytics events", Description = "Ensure tracking events are fired correctly.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(18) },
            new() { Title = "Write performance test scripts", Description = "Use k6 to script load tests for the API.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(35) },
            new() { Title = "Triage incoming bug reports", Description = "Reproduce and prioritize bugs reported by users.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(1) },
            new() { Title = "Create a QA status report", Description = "Summarize testing progress and bug metrics.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(6) },
            new() { Title = "Test email notifications", Description = "Verify that welcome and password reset emails are sent.", Status = TaskStatus.Completed, Priority = TaskPriority.Medium, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(14) },
            new() { Title = "Verify bug fixes from developers", Description = "Confirm that reported bugs have been resolved.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(2) },
            new() { Title = "Document testing procedures", Description = "Create a guide for new QA team members.", Status = TaskStatus.Completed, Priority = TaskPriority.Low, UserId = user5Id, DueDate = DateTime.UtcNow.AddDays(40) }
        };

        await context.Tasks.AddRangeAsync(tasks);
        await context.SaveChangesAsync();
    }
}