// IT Support Ticket Manager App


// Show a menu
Console.WriteLine("--- IT Support Ticket Manager ---");
Console.WriteLine(
	"1. Add Ticket\n2. Remove Ticket\n3. Display All Tickets\n4. Close Ticket\n5. Reopen Ticket\n6. Save Tickets to File\n7. Load Tickets from File\n8. Close"
	);

TicketManager tm = new();
bool continueRunning = true;

do
{
	int choice = GetInput<int>("Select an option: ", "Input must be a number!");

	switch (choice) {
		// Add ticket
		case 1:
			Console.WriteLine("- Adding a new ticket! -");
			AddTicket();
			WriteColoredLine("Ticket Added!", ConsoleColor.Green);
			break;
		
		// Remove ticket
		case 2:
			Console.WriteLine("- Removing a ticket! -");
			string removeId = GetInput<string>("Enter ticket ID to remove: ");
			if (tm.RemoveTicket(removeId))
				WriteColoredLine("Ticket Removed!", ConsoleColor.Green);
			else
				WriteColoredLine($"Could not find ticket '{removeId}'.", ConsoleColor.Red);
			break;
		
		// Display tickets
		case 3:
			Console.WriteLine("- Displaying all tickets! -");
			tm.DisplayAllTickets();
			break;
		
		// Close ticket
		case 4:
			Console.WriteLine("- Closing a ticket! -");
			string closeId = GetInput<string>("Enter ticket ID to close: ");
			Ticket? closeTicket = tm.FindTicket(closeId);
			if (closeTicket != null)
			{
				bool closed = closeTicket.CloseTicket();
				WriteColoredLine(closed ? "Ticket successfully closed!" : "Ticket is already closed.", closed ? ConsoleColor.Green : ConsoleColor.Red);
			}
			else
				WriteColoredLine($"Could not find ticket '{closeId}'.", ConsoleColor.Red);
				
			break;
		
		// Reopen ticket
		case 5:
			Console.WriteLine("- Reopening a ticket! -");
			string reopenId = GetInput<string>("Enter ticket ID to reopen: ");
			Ticket? reopenTicket = tm.FindTicket(reopenId);
			if (reopenTicket != null)
			{
				bool reopened = reopenTicket.ReopenTicket();
				WriteColoredLine(reopened ? "Ticket successfully reopened!" : "Ticket is already open.", reopened ? ConsoleColor.Green : ConsoleColor.Red);
			}
			else
				WriteColoredLine($"Could not find ticket '{reopenId}'.", ConsoleColor.Red);
				
			break;

		// Save tickets
		case 6:
			Console.WriteLine("- Saving tickets to a file! -");
			string savePath = GetInput<string>("Enter file path (relative): ");
			bool saved = tm.SaveTickets(savePath, out string saveError);
			if (saved)
				WriteColoredLine("Tickets were successfully saved!", ConsoleColor.Green);
			else
				WriteColoredLine(saveError, ConsoleColor.Red);
			break;
		
		// Load tickets
		case 7:
			Console.WriteLine("- Loading tickets from a file! -");
			string loadPath = GetInput<string>("Enter file path (relative): ");
			bool loaded = tm.LoadTickets(loadPath, out string loadError);
			if (loaded)
				WriteColoredLine("Tickets were successfully loaded!", ConsoleColor.Green);
			else
				WriteColoredLine(loadError, ConsoleColor.Red);
			break;

		
		// Exit
		case 8:
			Console.WriteLine("- Exiting! -");
			continueRunning = false;
			break;
		
		default:
			WriteColoredLine("Choice must be between 1 and 8!", ConsoleColor.Red);
			break;
	}




	
} while (continueRunning);


// Prompts the user to add a new ticket
void AddTicket()
{
	// Get a unique ID
	string id;
	while (true)
	{
		id = GetInput<string>("Enter ticket ID: ");

		// Check that the ID is valid and unique
		if (string.IsNullOrWhiteSpace(id)) {
			WriteColoredLine("ID cannot be empty!", ConsoleColor.Red);
			continue; }
		if (tm.FindTicket(id) != null) {
			WriteColoredLine($"There is already a ticket with the ID '{id}'! ID must be unique.", ConsoleColor.Red);
			continue; }
		break;
	}

	string description;
	while (string.IsNullOrWhiteSpace(description = GetInput<string>("Enter ticket description: ")))
		WriteColoredLine("Description cannot be empty!", ConsoleColor.Red);

	int intPriority;
	while ((intPriority = GetInput<int>("Enter priority level (1 - Low, 2 - Medium, 3 - High): ", "Input must be a number!")) < 1 || intPriority > 3)
		WriteColoredLine("Input must be between 1 and 3!", ConsoleColor.Red);
	Ticket.PriorityLevel priority = (Ticket.PriorityLevel)intPriority;
	
	// Create ticket object
	Ticket t = new(id, description, priority, Ticket.TicketStatus.Open);
	tm.AddTicket(t);
}

// Prompts the user to enter a value and returns the input as the specified type
static T GetInput<T>(string message, string? errorMessage = null) 
{
	while (true)
	{
		// Get input from user
		Console.Write(message);
		Console.ForegroundColor = ConsoleColor.Yellow;
		string input = Console.ReadLine() ?? "";
		Console.ResetColor();
		

		// Validate input and return
		if (typeof(T) == typeof(string))
			return (T)(object)input;
		if (typeof(T) == typeof(int) && int.TryParse(input, out int intValue))
			return (T)(object)(intValue);
		if (typeof(T) == typeof(double) && double.TryParse(input, out double doubleValue))
			return (T)(object)(doubleValue);

		// Print an error message
		WriteColoredLine(errorMessage ?? "Invalid Input! Please try again.", ConsoleColor.Red);
	}
}

// Writes a line to the console in the specified color
static void WriteColoredLine(string message, ConsoleColor color)
{
	Console.ForegroundColor = color;
	Console.WriteLine(message);
	Console.ResetColor();
}