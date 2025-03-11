# BistroQ - Restaurant Management System

## Links

- Milestone 1 Report: [https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/](https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/)
- Milestone 2 Report: [https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/HkhqJdrz1g](https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/HkhqJdrz1g)
- Milestone 3 Report: [https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/SywYnZt81l](https://hackmd.io/@CXS0caWySWi1obKmwFzL-Q/SywYnZt81l)

## Project Overview

BistroQ is a comprehensive restaurant management system built with WinUI 3 that provides a complete solution for restaurant operations. This application was developed as a project for the CSC13001 course - Windows Development from HCMUS.

The system connects to the [BistroQ API](https://github.com/tien4112004/bistro-q-api) to provide real-time data management and processing for restaurant operations across different roles - from administrators managing the restaurant setup to clients placing orders.

## Features

### Multi-role System

BistroQ supports four different user roles, each with specialized interfaces and capabilities:

- **Admin**: Comprehensive management of restaurant resources
- **Kitchen**: Order processing and preparation tracking
- **Cashier**: Table management and payment processing
- **Client**: Product browsing and order placement

### Admin Interface

- Zone management (create, edit, delete restaurant areas)
- Table management (create, edit, delete tables and assign to zones)
- Category management (organize products by categories)
- Product management (create, edit, delete menu items with prices and nutritional information)
- User account management (create and manage user accounts with role assignment)

### Kitchen Interface

- Kanban-style order management board
- Real-time order status tracking
- Order processing workflow (pending → in progress → completed)
- Order history and search capabilities

### Cashier Interface

- Zone and table overview with occupancy status
- Order details and payment processing
- Real-time checkout notifications
- Order tracking and management

### Client Interface

- Product browsing by categories
- Shopping cart functionality
- Nutritional information tracking
- Order placement and status tracking

### Additional Features

- Real-time notifications using SignalR
- Secure token-based authentication
- Responsive WinUI 3 interface
- Data pagination and sorting
- Form validation

## Architecture

BistroQ follows a clean, layered architecture:

### Domain Layer

- Contains entities, data transfer objects (DTOs), interfaces, and enums
- Defines the core business models and contracts

### Service Layer

- Implements API communication and data services
- Handles authentication and token management
- Provides real-time communication through SignalR

### Presentation Layer

- Implements the MVVM (Model-View-ViewModel) pattern
- Contains views, view models, and UI components
- Manages user interactions and state

## Technologies Used

- **WinUI 3**: Modern UI framework for Windows applications
- **.NET 8.0**: Latest .NET platform
- **MVVM Pattern**: Using CommunityToolkit.Mvvm for clean separation of concerns
- **SignalR**: For real-time communication between clients and server
- **AutoMapper**: For object mapping between DTOs and domain models
- **RESTful API Integration**: For data exchange with the backend
- **Dependency Injection**: For loose coupling and better testability

## Design Patterns

- **MVVM**: For separation of UI and business logic
- **Repository Pattern**: For data access abstraction
- **Strategy Pattern**: For kitchen order actions
- **Observer Pattern**: Via Messenger for component communication
- **Factory Pattern**: For creating strategy instances

## Getting Started

### Prerequisites

- Windows 10 version 1809 or later
- .NET 8.0 SDK
- Visual Studio 2022 with Windows App SDK workload

### Installation

1. Clone this repository
2. Clone the [BistroQ API](https://github.com/tien4112004/bistro-q-api) repository and follow its setup instructions
3. Open `BistroQ.sln` in Visual Studio 2022
4. Update the API endpoint in `App.xaml.cs` if necessary
5. Build and run the application

## User Roles and Access

### Admin

- Full access to all management features
- Can create and manage user accounts
- Can configure restaurant layout (zones, tables)
- Can manage menu (categories, products)

### Kitchen

- Access to order preparation interface
- Can update order status (pending, in progress, completed)
- Can view order history

### Cashier

- Access to table management interface
- Can view table status and order details
- Can process payments and checkout

### Client

- Access to product browsing and ordering interface
- Can add items to cart and place orders
- Can view nutritional information

## Project Structure

- **BistroQ.Domain**: Contains entities, DTOs, contracts, and business logic
- **BistroQ.Service**: Implements data services and API clients
- **BistroQ.Presentation**: Contains ViewModels, Views, and UI components
- **BistroQ.Tests.MsTest**: Contains unit tests

## Development Notes

- The application uses the MVVM pattern with CommunityToolkit.Mvvm
- ViewModels implement INavigationAware for navigation handling
- State management is handled through dedicated State classes
- Messaging between components is implemented using CommunityToolkit.Mvvm.Messaging
- Form validation is implemented through ValidatorBase class

## Academic Context

This project was developed as part of the CSC13001 - Windows Development course at HCMUS (Ho Chi Minh City University of Science). It demonstrates the application of:

- WinUI 3 development principles
- Clean architecture and design patterns
- API integration and real-time communication
- User experience design for different user roles
- Security practices for user authentication

## License

This project is provided for educational purposes as part of the CSC13001 course.
