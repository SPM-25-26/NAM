# eppoi

**eppoi** is a Progressive Web App (PWA) designed to transform the tourism experience in the city of **Matelica**. By leveraging Artificial Intelligence, the app provides personalized itinerary recommendations and an intelligent virtual assistant to guide visitors through the city's cultural, gastronomic, and scenic highlights.

> *Discover Matelica like never before—personalized, intelligent, and always with you.*

## Table of Contents

* [Introduction](#introduction)
* [Features](#features)
* [Architecture](#architecture)
* [Installation](#installation)
* [Usage](#usage)
* [Technology Stack](#technology-stack)
* [Contributing](#contriguting)
* [Code of Conduct](#code-of-conduct)
* [License](#license)
* [Authors](#authors)

## Introduction

Tourism in Matelica has a rich history, but finding the right experiences can be overwhelming. **eppoi** solves this by acting as a smart travel companion. Whether a user is looking for the best Verdicchio wine tasting, a historical tour of the Palazzo Ottoni, or a hidden nature trail, **eppoi** learns from their preferences to suggest the perfect activity.

This repository contains the source code for the **NAM** group for the academic year 2025-2026.

## Features

* **Progressive Web App (PWA):** Installable directly from the browser on both Android and iOS. Works offline and ensures high performance.
* **AI-Powered Assistant:** A conversational interface that answers questions about Matelica, from opening hours to local legends.
* **Personalized Recommendations:** An intelligent engine suggesting itineraries and events based on user interests and personality.
* **Cloud-Native Backend:** Built with .NET Aspire for resilience, observability, and scalability.

## Installation

This project uses a distributed architecture orchestrated by .NET Aspire. Follow these steps to set up the development environment.

### Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/it-it/download/dotnet/9.0) (or higher)
* [Docker Desktop](https://www.docker.com/) (required for running Aspire containers)
* [Node.js](https://nodejs.org/en) (for the frontend PWA)

### Steps

1. **Clone the repository:**
```bash
git clone https://github.com/SPM-25-26/NAM.git
cd NAM

```


2. **Configure Environment Variables:**
Each subproject in the solution contains a `.env.example` file. You must configure these individually.
* Navigate to each service directory.
* Copy `.env.example` to `.env`.
* Fill in the required keys (API keys, connection strings, etc.) in each `.env` file.


```bash
# Example for one service
cp nam.Server/.env.example nam.Server/.env

```


3. **Trust the Development Certificate:**
Ensure your local HTTPS development certificates are trusted.
```bash
dotnet dev-certs https --trust

```


4. **Run the Application:**
Launch the **AppHost** project. This will orchestrate all frontend, backend, and database containers.
```bash
# Run from the root solution folder, targeting the AppHost project
dotnet run --project nam.AppHost

```


5. **Access the Dashboard:**
Once running, the console will provide a link to the **.NET Aspire Dashboard** (usually `https://localhost:17149`). From there, you can view logs, traces, and access the PWA endpoint.

## Usage

1. **Onboarding:** Open the app and complete the initial preference survey to help the AI understand your interests.
2. **Ask the AI:** Click on the chat icon to ask questions like "Where can I eat traditional Vincisgrassi?"
3. **Install PWA:**
* **Mobile:** Tap "Share" (iOS) or the menu dots (Android) and select "Add to Home Screen."
* **Desktop:** Click the install icon in the browser address bar.



## Technology Stack

* **Orchestration:** [.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/)
* **Backend:** C# / .NET 9+
* **Frontend (PWA):** [React / Blazor WASM / Vue]
* **AI Integration:** [e.g., OpenAI API / Semantic Kernel]
* **Database:** [e.g., SQL Server / Qdrant] (Managed via Aspire components)
* **Observability:** OpenTelemetry (Built-in with Aspire)

## Contributing

We welcome contributions to make **eppoi** better for everyone!

1. **Fork** the repository.
2. Create a **Feature Branch** (`git checkout -b feature/AmazingFeature`).
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`).
4. **Push** to the branch (`git push origin feature/AmazingFeature`).
5. Open a **Pull Request**.

Please ensure your code adheres to the project's coding standards.

## License

This project is licensed under the **MIT License** - see the [LICENSE](https://www.google.com/search?q=LICENSE) file for details.

## Authors

* **[Nicol Buratti]** - *Role* - [@nicol-buratti](https://github.com/nicol-buratti)
* **[Antonio Marseglia]** - *Role* - [@AntoMars14](https://github.com/AntoMars14)
* **[Matteo Brachetta]** - *Role* - [@BrachettaMatteo](https://github.com/BrachettaMatteo)

**SPM-25-26** - *University Of Camerino*
