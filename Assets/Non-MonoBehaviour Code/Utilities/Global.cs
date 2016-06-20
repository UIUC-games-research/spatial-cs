using System;

public static class Global {
	public static T NonNull <T> (T toCheck, string assertionFailedMessage) where T : class {
		if (toCheck == null)
			throw new ArgumentNullException(assertionFailedMessage);
		return toCheck;
	}
}
