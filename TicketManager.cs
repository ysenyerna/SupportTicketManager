// Class that holds and manages a list of tickets

public class TicketManger
{
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

	// Saves ticket data to a CSV file at the specified path
	public void SaveTickets(string path)
	{
		List<string> ticketData = [];

		// Add header row
		ticketData.Add("Id,Description,Priority,Status,DateCreated");

		// Add each ticket as its own row
		foreach (Ticket t in _tickets)
		{
			string line = $"{t.ID},{t.Description},{t.Priority},{t.Status},{t.DateCreated}";
			ticketData.Add(line);
		}

		// Validate the file path and write to the file
		try
		{
			// If the user entered a directory, add a file name
			if (Directory.Exists(path))
				path = Path.Combine(path, "tickets.csv") ;

			File.WriteAllLines(Path.ChangeExtension(path, ".csv"), ticketData);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Could not save file: {ex.Message}");
		}
		
	}

	// Loads ticket data from a CSV file at the specified path
	public void LoadTickets(string path)
	{
		string[] ticketData = File.ReadAllLines(path);
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

				// Add the ticket
				Ticket t = new(id, description, priority, status, dateCreated);
				AddTicket(t);

			} 
			// Catch value errors
			catch (Exception ex)
			{
				Console.WriteLine($"Skipped line {lineNumber}: {ex.Message}");
			}
			lineNumber++;
			
		}
	}
}