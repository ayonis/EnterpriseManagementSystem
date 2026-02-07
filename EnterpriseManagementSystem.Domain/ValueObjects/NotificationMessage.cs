namespace EnterpriseManagementSystem.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing a notification message
/// </summary>
public sealed class NotificationMessage
{
    public string Recipient { get; }
    public string Subject { get; }
    public string Body { get; }

    private NotificationMessage(string recipient, string subject, string body)
    {
        Recipient = recipient;
        Subject = subject;
        Body = body;
    }

    /// <summary>
    /// Creates a new NotificationMessage instance with validation
    /// </summary>
    /// <param name="recipient">The recipient address (email or phone number)</param>
    /// <param name="subject">The message subject</param>
    /// <param name="body">The message body</param>
    /// <returns>A validated NotificationMessage instance</returns>
    /// <exception cref="ArgumentException">Thrown when any parameter is null or empty</exception>
    public static NotificationMessage Create(string recipient, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(recipient));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be null or empty", nameof(body));

        return new NotificationMessage(recipient.Trim(), subject.Trim(), body.Trim());
    }

    /// <summary>
    /// Creates a NotificationMessage without subject (useful for SMS)
    /// </summary>
    /// <param name="recipient">The recipient address</param>
    /// <param name="body">The message body</param>
    /// <returns>A validated NotificationMessage instance</returns>
    public static NotificationMessage CreateWithoutSubject(string recipient, string body)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(recipient));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Body cannot be null or empty", nameof(body));

        return new NotificationMessage(recipient.Trim(), string.Empty, body.Trim());
    }

    public override bool Equals(object? obj)
    {
        if (obj is not NotificationMessage other)
            return false;

        return Recipient == other.Recipient &&
               Subject == other.Subject &&
               Body == other.Body;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Recipient, Subject, Body);
    }

    public override string ToString()
    {
        return $"Recipient: {Recipient}, Subject: {Subject}, Body: {Body[..Math.Min(50, Body.Length)]}...";
    }
}

