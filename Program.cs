// IT Support Ticket Manager App

Console.WriteLine("--- IT Support Ticket Manager ---");

Ticket t = new("T1001", "Printer not working", Ticket.PriorityLevel.High, Ticket.TicketStatus.Open);
Console.WriteLine(t.GetSummary());
