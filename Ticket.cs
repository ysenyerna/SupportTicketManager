// Class for individual support tickets

public class Ticket
{
	// ID
	string _id = "";
	public string Id { get { return _id; } private set {
			if (string.IsNullOrWhiteSpace(value.Trim()))
				throw new ArgumentException("ID cannot be empty");
			_id = value.Trim();
		}
	}

	// Description 
	private string _description = "";
	public string Description { get {return _description; } private set
		{
			if (string.IsNullOrWhiteSpace(value.Trim()))
				throw new ArgumentException("Description cannot be empty");
			_description = value.Trim();
		}
	}

	// Priority level
	public enum PriorityLevel { Low = 1, Medium = 2, High = 3 }
	public PriorityLevel Priority { get; private set; }
	// Status
	public enum TicketStatus { Open, InProgress, Closed }
	public TicketStatus Status { get; private set; }
	// Creation date
	public DateTime DateCreated { get; private set; }

	// Constructors
	public Ticket() : this("N/A", "N/A", PriorityLevel.Low, TicketStatus.Open) {}

	public Ticket(string id, string description, PriorityLevel priority, TicketStatus status, DateTime? dateCreated = null)
	{
		Id = id;
		Description = description;
		Priority = priority;
		Status = status;
		DateCreated = dateCreated ?? DateTime.Now;
	}

	// Methods

	// Closes the ticket, returns false if the ticket is already closed
	public bool CloseTicket()
	{
		bool alreadyClosed = Status == TicketStatus.Closed;
		Status = TicketStatus.Closed;
		return !alreadyClosed;
	}

	// Reopens the ticket, returns false if the ticket is already open
	public bool ReopenTicket()
	{
		bool alreadyOpen = Status == TicketStatus.Open;
		Status = TicketStatus.Open;
		return !alreadyOpen;
	}

	// Returns a formatted summary of the ticket
	public string GetSummary()
	{
		return $"[{Id}] ({Priority}) - {Description} | Status: {Status} | Created: {DateCreated:yyyy-MM-dd}";
	}
}