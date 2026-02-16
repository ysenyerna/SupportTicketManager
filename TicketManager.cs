// Class that holds and manages a list of tickets

public class TicketManager
{
	const string CommaPlaceholder = "[COMMA]";
	public List<Ticket> _tickets {get; private set; } = [];

	// Methods

	// Adds a new ticket
	public void AddTicket(Ticket t)
	{
		_tickets.Add(t);
	}

	// Attempts to remove a ticket by ID, returns true if successful
	public bool RemoveTicket(string id)
	{
		Ticket? t = _tickets.Find(t => t.ID == id);
		return t == null ? false : _tickets.Remove(t);
	}

	// Find and return a ticket by ID, returns null if no ticket is found
	public Ticket? FindTicket(string id)
	{
		return _tickets.Find(t => t.ID == id);
	}

	// Displays summaries of all tickets
	public void DisplayAllTickets()
	{
		if (!_tickets.Any()) {
			Console.WriteLine("The ticket system is currently empty!");
			return; }
		_tickets.ForEach(t => Console.WriteLine(t.GetSummary()));
	}

	// Returns the amount of open tickets
	public int GetOpenCount()
	{
		return _tickets.Count(t => t.Status == Ticket.TicketStatus.Open);
	}

	// Saves ticket data to a CSV file at the specified path, returns false and outputs an error message if unsuccessful
	public bool SaveTickets(string path, out string errorMessage)
	{
		errorMessage = "";
		List<string> ticketData = [];

		// Add header row
		ticketData.Add("Id,Description,Priority,Status,DateCreated");

		// Add each ticket as its own row
		foreach (Ticket t in _tickets)
		{
			// Replace commas with a placeholder to avoid issues when reading files
			string id = t.ID.Replace(",", CommaPlaceholder);
			string description = t.Description.Replace(",", CommaPlaceholder);
			string line = $"{id},{description},{t.Priority},{t.Status},{t.DateCreated}";
			ticketData.Add(line);
		}

		// Validate the file path and write to the file
		try
		{
			// If the user entered a directory, add a file name
			if (Directory.Exists(path))
				path = Path.Combine(path, "tickets.csv") ;

			File.WriteAllLines(Path.ChangeExtension(path, ".csv"), ticketData);
			return true;
		}
		catch (Exception ex)
		{
			errorMessage = $"Could not save file: {ex.Message}";
			return false;
		}
		
	}

	// Loads ticket data from a CSV file at the specified path, returns false and outputs an error message if unsuccessful
	public bool LoadTickets(string path, out string errorMessage)
	{
		errorMessage = "";

		// Load the file
		string[] ticketData;
		try
		{
			ticketData = File.ReadAllLines(path);
		}
		catch (Exception ex) {
			errorMessage = $"Failed to load file: {ex.Message}";
			return false;
		}
		
		_tickets.Clear();

		// Iterate through file data
		int lineNumber = 1;
		foreach (string line in ticketData.Skip(1))
		{
			string[] values = line.Split(",");

			try
			{
				// Parse the values from the CSV file
				string id = values[0];
				string description = values[1];
				Ticket.PriorityLevel priority = Enum.Parse<Ticket.PriorityLevel>(values[2]);
				Ticket.TicketStatus status = Enum.Parse<Ticket.TicketStatus>(values[3]);
				DateTime dateCreated = DateTime.Parse(values[4]);

				// Put the commas back in
				id = id.Replace(CommaPlaceholder, ",");
				description = description.Replace(CommaPlaceholder, ",");

				// Add the ticket
				Ticket t = new(id, description, priority, status, dateCreated);
				AddTicket(t);

			} 
			// Catch value errors
			catch (Exception ex)
			{
				errorMessage = $"Skipped line {lineNumber}: {ex.Message}";
				return false;
			}
			lineNumber++;
		}
		return true;
	}
}