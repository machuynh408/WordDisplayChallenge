# WordDisplayChallenge

Instruction:
  - cd /WordDisplayChallenge/WordDisplayServer
  - build a docker image with [docker build -t wsd:v1 .]
  - run a container with the image [docker run -it --rm -p 5555:80 wsd:v1]
  - cd /WordDisplayChallenge/WordDisplay/WordDisplay
  - start blazor with [dotnet run]
  - on the browser, navigate to https://localhost:5001/

Goal Stretch:
  - I had issues with getting the database to work with PostgreSQL, so I opted in for MariaDB.
  - I have the DbContext for Logging all set up. However, I couldn't figure out why entries weren't being inserted into the tables.
  - Approach:
    - I simply would just need to register the LoggingContext which is a DbContext.
    - Inject the DbContext using Dependency Injection, and make the necessary insert calls to insert the Command and State models in my Word Service.
      - The Command model is inserted when the Start command is invoked.
      - The State model is inserted every second during a cycle for the word as its state is constantly changing.
    - Thus, I was conflicted with time for logging portion, as I would also have to make sure the installation and dependencies setup for the Docker container works.

Kubernetes:
  - I didn't get a chance to tackle this.
  - It is used for container orchestration. In a scenario where my containerized API Server goes down, Kubernetes will hopefully make sure to bring my service back up.
  - Approach:
    - For me to test this, I would need to write some code on the client side to intentionally break the API Server, for example, a bad payload or request.
    - Thus, it will allow me to see whether or not Kubernetes is doing its duty to bring my service back up.


Final Thoughts:
  - The challenge was a lot fun as I learned a lot about ASP.NET Core and Blazor as it was technologies I have never used in full stack development.
  - I also learned about SignalR which helped in me building the real-time app this was aimed for.
  - Due to time, I also wasn't able to write the necessary Unit testing which is needed for production ready code.
