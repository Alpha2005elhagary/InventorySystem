# 📦 Inventory Management System (ASP.NET Core MVC).

Architecture skeleton for an **Inventory Management System** built with **ASP.NET Core MVC** and **Entity Framework Core**.

---

## 🏗️ Architecture Structure

```
InventorySystem/
├── Controllers/                 # MVC Controllers
│   └── .gitkeep
├── Data/                        # EF Core DbContext & Configurations
│   └── .gitkeep
├── Models/                      # Domain & Entity Models
│   └── .gitkeep
├── ViewModels/                  # ViewModels for presentation views
│   └── .gitkeep
├── Views/                       # Razor Views
│   ├── Account/
│   │   └── .gitkeep
│   ├── Categories/
│   │   └── .gitkeep
│   ├── Home/
│   │   └── .gitkeep
│   ├── Products/
│   │   └── .gitkeep
│   ├── Shared/
│   │   └── .gitkeep
│   ├── Stock/
│   │   └── .gitkeep
│   └── Suppliers/
│       └── .gitkeep
└── wwwroot/                     # Static Web Assets
    ├── css/
    │   └── .gitkeep
    ├── images/
    │   └── .gitkeep
    └── js/
        └── .gitkeep
```

---

## 🗄️ Database Design

- **Category**: One-to-Many relationship with `Product`
- **Supplier**: One-to-Many relationship with `Product`
- **Product**: One-to-Many relationship with `StockTransaction`
