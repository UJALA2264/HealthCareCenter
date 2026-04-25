🚀 Overview

A cloud-integrated healthcare application built with .NET Core and Azure, enabling seamless doctor-patient interaction with automated workflows and scalable storage.

Designed to move beyond basic CRUD and demonstrate real-world Azure service integration.

✨ Core Features

👨‍⚕️ Doctor

Manage upcoming appointments (next 7 days)
Accept / Reject requests
Profile management

🧑‍💻 Patient

Search doctors by specialization, name, or clinic
Book appointments
Track pending & appointment history
☁️ Azure in Action
App Service → Hosts frontend & APIs
Azure SQL Database → Reliable data storage
Blob Storage → Doctor image management
Azure Functions → Serverless image upload handling
Logic Apps → Automated email notifications
🔄 System Flow

Patient books → Doctor reviews → Action taken →
Logic App triggers notification →
Images handled via Function → Blob Storage

🛠️ Tech Stack

.NET Core • ASP.NET MVC • Web API • SQL Server • Azure

💡 What Makes This Project Stand Out
✔ End-to-end Azure integration
✔ Real use of serverless + automation
✔ Practical cloud architecture implementation
✔ Focus on scalability and real-world workflow
