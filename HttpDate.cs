/// <summary>
/// Date formats for dates passed within an HTTP request/response
/// </summary>
public static class HttpDate
{
	/// <summary>
	/// RFC 822, updated by RFC 1123 (Sun, 06 Nov 1994 08:49:37 GMT)
	/// </summary>
	private const string Rfc1123Format = @"ddd, dd MMM yyyy HH:mm:ss G\MT";

	/// <summary>
	/// RFC 850, obsoleted by RFC 1036 (Sunday, 06-Nov-94 08:49:37 GMT)
	/// </summary>
	private const string Rfc850Format = @"dddd, dd-MMM-yy HH:mm:ss G\MT";

	/// <summary>
	/// ANSI C's asctime() format (Sun Nov  6 08:49:37 1994)
	/// </summary>
	private const string AscTimeFormat = "ddd MMM d HH:mm:ss yyyy";

	/// <summary>
	/// Parses a date value received over HTTP
	/// http://www.w3.org/Protocols/rfc2616/rfc2616-sec3.html#sec3.3.1
	/// </summary>
	/// <param name="value">Value to parse</param>
	/// <param name="timestamp">Parsed date, if pasing succeeded</param>
	/// <returns>True is parsing succeeds</returns>
	public static bool TryParse(string value, out DateTime timestamp)
	{
		if (value == null)
			throw new ArgumentNullException(nameof(value));

		var parsed = DateTime.TryParseExact(value, Rfc1123Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);

		if (!parsed)
			parsed = DateTime.TryParseExact(value, Rfc850Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);

		if (!parsed)
		{
			value = value.Replace("  ", " "); // remove double spaces (between month and day for single digit days)
			parsed = DateTime.TryParseExact(value, AscTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);
		}

		return parsed;
	}

	/// <summary>
	/// Formats the date based on RFC1123
	/// </summary>
	/// <param name="utc">UTC date to format</param>
	/// <returns>Formatted date</returns>
	public static string Format(DateTime utc)
	{
		return FormatRfc1123(utc);
	}

	/// <summary>
	/// Formats the date based on RFC1123
	/// </summary>
	/// <param name="utc">UTC date to format</param>
	/// <returns>Formatted date</returns>
	public static string FormatRfc1123(DateTime utc)
	{
		return utc.ToString(Rfc1123Format, CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Formats the date based on RFC850
	/// </summary>
	/// <param name="utc">UTC date to format</param>
	/// <returns>Formatted date</returns>
	public static string FormatRfc850(DateTime utc)
	{
		return utc.ToString(Rfc850Format, CultureInfo.InvariantCulture);
	}

	/// <summary>
	/// Formats the date based on ASC TIME
	/// </summary>
	/// <param name="utc">UTC date to format</param>
	/// <returns>Formatted date</returns>
	public static string FormatAscTime(DateTime utc)
	{
		return utc.ToString(AscTimeFormat, CultureInfo.InvariantCulture);
	}
}