# ToDo App using ASP.NET Core MVC

This is a simple ToDo application built using ASP.NET Core MVC framework. It allows you to create, read, update and delete tasks.

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed on your machine.
- An IDE (Visual Studio, Visual Studio Code) for running and modifying the code.

### Installing

1. Clone the repository

```bash
git clone https://github.com/nkathawa/Todo.git
```

2. Open the project in your IDE of choice.

3. Run the application using dotnet run command.

```bash
cd Todo
dotnet run
Open your browser and navigate to http://localhost:5087 to view the application.
```

### Running in GCP (Production)

1. `cd Todo/`
2. `git pull`
3. `sudo docker build -t todo .`
4. `sudo docker run -d -p 80:80 todo`

If needed, you can stop the container using `sudo docker stop <container_id>` and remove it using `sudo docker rm <container_id>`. Then, you can rebuild and run the container using the commands above. Command `sudo docker ps` will show you the running containers.

### Accessing the Application in Production

1. Navigate to http://35.211.34.128/
2. Create a new account and make your own todo items!

### Features
- Create, read, update and delete tasks.
- Mark a task as completed.
- Filter tasks by status (completed or incomplete).

### Technologies Used
- C#
- ASP.NET Core MVC
- Entity Framework Core
- Bootstrap
- RESTful APIs
- Razor Pages
- cshtml

### Contributing
If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are welcome.

### License
No license for this project.

### Acknowledgments
Microsoft Documentation for providing an excellent tutorial on ASP.NET Core MVC.