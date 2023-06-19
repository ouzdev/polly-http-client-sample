# Sample Project with Polly in .NET 6

This sample project demonstrates a Server and Client architecture using .NET 6 and Polly library. The Server is a .NET Web API project that implements JWT Bearer authentication for authorization. The Client is a .NET Console application that communicates with the Server using Polly library.

## Project Structure

- **Server**: Represents a .NET Web API server. The server simulates operations with a limited duration and applies error handling and resilience strategies using the Polly library.

- **Client**: Represents a .NET Web API client that interacts with the Server. The client utilizes the Polly library to retrieve data from the server or send data to it.

## Technologies Used

- .NET 6: The latest version of .NET Framework for developing applications.

- Polly: An open-source .NET library for handling errors and implementing resilience strategies. Polly can be used to handle network errors, timeouts, error codes, and more.

- JWT Bearer Authentication: A token-based authentication method using JSON Web Tokens (JWT). JWT Bearer Authentication is employed for user authorization and authentication in this project.
