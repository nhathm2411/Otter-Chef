# Otter-Chef 🍜

Otter-Chef is a Korean food ordering web application built with a microservices architecture using ASP.NET Core. It allows users to browse dishes, add items to a shopping cart, place orders, and receive notifications – all through a responsive and user-friendly interface.

---

## 🛠️ Technologies

### Front-end:
- HTML, CSS, Bootstrap
- JavaScript
- Razor Pages (ASP.NET Core MVC View)

### Back-end:
- ASP.NET Core MVC
- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server

### Others:
- Microservices Architecture
- API Gateway (Routing)
- JWT Authentication
- MailKit (Email Notifications)
- Dependency Injection

---

## 🔑 Key Features

- 🔐 User authentication & authorization  
- 🥘 Product browsing & filtering (Korean dishes)  
- 🛒 Shopping cart functionality  
- 📦 Order placement & tracking  
- 📧 Email confirmation via MailKit  
- 🌐 API Gateway for centralized routing  

---

## 🧱 Microservices Overview

- **AuthService**: Handles registration, login, and JWT token issuing  
- **ProductService**: Manages product data (menu items)  
- **CartService**: Handles shopping cart operations  
- **OrderService**: Manages customer orders  
- **EmailService**: Sends order confirmation and notifications  
- **API Gateway**: Routes requests to services securely
