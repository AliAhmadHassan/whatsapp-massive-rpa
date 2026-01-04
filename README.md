# WhatsApp Massive RPA

Automates WhatsApp messaging in bulk so organizations can publish updates without adding a single recipient manually. This toolkit connects a Windows Forms surface, a supporting console runner, remote REST APIs, and Selenium-driven WhatsApp Web flows to orchestrate high-volume campaigns while tracking delivery status for every contact.

## Architecture at a glance

- **RoboWhatsApp.UI**  
  Windows Forms front-end that boots a `ChromeDriver`, tracks login/readiness states in WhatsApp Web, and triggers the batch-send workflow. It leverages `SeleniumHelper` to poll the DOM and guard transitions between the login screen and the main chat list.
- **RobotWhatsApp.Console**  
  Headless automation alternative that pairs Selenium with native Win32 hooks (`SetCursorPos`, `mouse_event`, `SendMessage`, pixel sampling) so the same drive routines can integrate with Genymotion emulators or other windowed clients when WhatsApp Web needs precise targeting.
- **RoboWhatsApp.DAL**  
  A synchronous-friendly data-access layer that talks to a local REST service (configured through `EASY_API.path`, default `http://localhost:8080/`). Every resource (contacts, messages, lots, companies) exposes CRUD plus campaign-specific mutations like `loteParaEnvio`, `setSingleCheck`, and `setDoubleCheck`.
- **RoboWhatsApp.DTO**  
  Rich domain models (`Contato`, `Mensagem`, `Lote`, `Empresa`, `Token`, `Response<T>`, `Pages<T>`, etc.) share clean shape definitions across UI, console, and DAL projects so JSON serialization is consistent and easy to mock in tests.

## What it can do

- Bootstrap WhatsApp Web sessions via Selenium, detect when QR login is ready, and wait for the main chat screen to avoid flaky timing.
- Fetch batches of contacts and lots from the REST backend and run through them sequentially, optionally filtering only WhatsApp-enabled recipients.
- Send text and media payloads (`SendWhatsMessage`) through WhatsApp Web, including attaching files by automating the OS file picker window and clicking the send button once uploads finish.
- Track status updates (`setEnvio`, `setSingleCheck`, `setDoubleCheck`) so the backend can mark messages that were sent, received, or delivered.
- Select every valid contact in a lot, create a new chat, and optionally log successes (for example, file writes under `C:\temp` in commented code) when a contact exists on WhatsApp.

## Technical highlights

- **Selenium + ChromeDriver** drive the automation without running a headless browser, combining DOM queries (`FindElementsByClassName`, XPath) with helper methods that poll `PageSource` for stability.
- **HTTP client layer** uses `HttpClient` wrappers (`ApiClient`) with JSON serialization (`Microsoft.AspNet.WebApi.Client`, `Newtonsoft.Json`) to keep service contracts simple while easily swapping base endpoints.
- **Win32 integration** inside `RobotWhatsApp.Console` and `SendWhatsMessage` (`FindWindow`, `SetForegroundWindow`, pixel inspection) enables screenshot-free UI automation for scenarios where Selenium alone struggles (legacy apps or emulators).
- **Async/await** spans the DAL so calls like `loteParaEnvio` and `findById` can execute without blocking the UI thread while still exposing synchronous entry points via `.GetAwaiter().GetResult()` when needed.
- **Modular solution (`RoboWhatsApp.sln`)** splits UI, console, DAL, and DTO to mirror clean architecture principles: presentation, automation, data access, and contracts are decoupled for easier maintenance.

## Running the project

1. **Restore and build** `RoboWhatsApp.sln` with Visual Studio 2015/2017 or `msbuild` targeting .NET 4.5; each project relies on the packages listed in `packages.config` (Selenium, Newtonsoft.Json, Microsoft.Bcl, Web API client).
2. **Host the backend** so `EASY_API.path` points to your API (default `http://localhost:8080/`). Ensure the REST endpoints (`api/contato`, `api/mensagem`, etc.) exist and honor the contract defined in the DTO layer.
3. **Prepare ChromeDriver** (matching the installed Chrome version) and let NuGet restore `Selenium.WebDriver.ChromeDriver`. The automation opens `https://web.whatsapp.com/` and requires a logged-in session.
4. **Configure data**: seed lots, contacts, messages, and companies through the backend. Each `Contato` carries `ddi`, `ddd`, `numeroTelefone`, and status metadata so the automation can publish only valid peers.
5. **Start the UI or console runner**: the form app lets you trigger flows manually; the console runner can be scheduled or invoked with emulators, especially when tying native window behavior into the automation.

## Notes for recruiters

- Built a bespoke RPA solution covering frontend automation, backend integration, and native Windows APIs: an end-to-end pipeline for high-volume messaging.
- The layered solution demonstrates design discipline: DTOs share contracts, DAL centralizes REST plumbing, and UI/console runners focus purely on automation concerns.
- Challenges faced: synchronizing Selenium with WhatsApp's dynamic DOM, coordinating file uploads through native dialogs, and proving delivery states back to the backend.

## Next steps you might consider

- Replace hard-coded window names and pixel coordinates with accessibility hooks or UI Automation APIs so the console runner adapts to new screen layouts.
- Add configuration-driven waits (instead of `Thread.Sleep`) plus retry policies for file uploads, making the bot more resilient to WhatsApp UI changes.
- Introduce unit/integration tests around the DAL contracts and mock Selenium interactions to safely refactor the automation logic.
