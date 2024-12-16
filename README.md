# bistro-q

## Introduction

> [!CAUTION]
> Please edit here @Tondeptrai23

## Technical stack

- .NET 8.0
- WinUI 3 with XAML

## Architecture

We have decided to reconstruct our WinUI project to follow layered architecture. The folder structure shown below. We can clearly see that each layer doing its job.

```
.
├── BistroQ.Domain
│   ├── Contracts
│   │   ├── Common
│   │   ├── Http
│   │   └── Services
│   ├── Dtos
│   │   ├── Auth
│   │   ├── Category
│   │   ├── Common
│   │   ├── Orders
│   │   ├── Products
│   │   ├── Tables
│   │   └── Zones
│   ├── Enums
│   ├── Helpers
│   └── Models
│       ├── Entities
│       └── Exceptions
├── BistroQ.Presentation
│   ├── Activation
│   ├── Assets
│   │   └── Icons
│   ├── Behaviors
│   ├── Contracts
│   │   ├── Services
│   │   └── ViewModels
│   ├── Controls
│   │   └── Skeleton
│   ├── Converters
│   ├── Enums
│   ├── Helpers
│   ├── Mappings
│   ├── Messages
│   ├── Models
│   ├── Properties
│   ├── Services
│   ├── Strings
│   │   └── en-us
│   ├── Styles
│   ├── Validation
│   ├── ViewModels
│   │   ├── AdminTable
│   │   ├── AdminZone
│   │   ├── CashierTable
│   │   ├── Client
│   │   ├── Commons
│   │   ├── KitchenHistory
│   │   ├── KitchenOrder
│   │   ├── Models
│   │   └── States
│   └── Views
│       ├── AdminTable
│       ├── AdminZone
│       ├── CashierTable
│       ├── Client
│       ├── KitchenHistory
│       ├── KitchenOrder
│       └── UserControls
├── BistroQ.Service
│   ├── Auth
│   ├── Common
│   ├── Data
│   └── Http
├── BistroQ.Tests.MsTest
```

## Setup

- Ensure that [backend project](https://github.com/tien4112004/bistro-q-api) has been set up correctly. 
- Open your IDE (Visual Studio is recommended), and run `BistroQ.Presentation` project. 