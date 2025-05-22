# 📚 Library Management System

A full-featured **Library Management System** built using **ASP.NET Web Forms** (C#) and **Microsoft SQL Server**. This web application supports both admin and user functionalities like managing books, issuing books, managing members, login/signup, and more.

---

## 🛠️ Tech Stack

- **Frontend:** ASP.NET Web Forms (ASPX), HTML, CSS  
- **Backend:** C# (code-behind), ADO.NET for database operations  
- **Database:** Microsoft SQL Server  
- **IDE:** Visual Studio  
- **Authentication:** Session-based (Admin/User)  
- **Architecture:** Web Forms with Master Pages for layout consistency

---

## 📁 Project Structure

Here is a breakdown of the key pages and their purpose:

### 🔐 Authentication

- `adminlogin.aspx` — Admin login page  
- `userlogin.aspx` — User login page  
- `usersignup.aspx` — User registration/signup

### 👨‍💼 Admin Pages

- `adminauthormanagement.aspx` — Manage book authors  
- `adminpublishermanagement.aspx` — Manage publishers  
- `adminbookinventory.aspx` — Add/update/remove books  
- `adminbookissuing.aspx` — Issue/return books to/from users  
- `adminmembermanagement.aspx` — Manage library members

### 👤 User Pages

- `userprofile.aspx` — View and update user profile  
- `viewbooks.aspx` — Browse available books  
- `home.aspx` — User homepage/dashboard

### 🧱 Shared/Layout

- `Site1.Master` — Master page for consistent layout across the site  
- `Web.config` — Project configuration and database connection settings

---

## ✅ Features

### Admin

- Login to admin dashboard  
- Add, update, or delete books  
- Manage authors and publishers  
- Issue/return books  
- View and manage registered users

### User

- Sign up and log in  
- View and update personal profile  
- Browse books  
- View book availability and details
- Rent books
---

## 🚀 Getting Started

### Prerequisites

- Visual Studio
- SQL Server / SQL Server Express

### Steps

1. **Clone the Repository**
2. **Open `LibraryManagement.csproj`** in Visual Studio
3. **Set up your database** using the provided schema (or manually create the required tables)
4. **Update `Web.config`** with your database connection string
5. **Run the project** in Visual Studio (Start Debugging)

---

## 🔐 Default Credentials (Example)

> These may differ in your implementation

**Admin**  
- Create an admin in the database

**User**  
- Create account via the signup page

---



