﻿namespace SharedKernel.Infrastructure.PayPal.Api;

/// <summary>
/// Represents a PayPal model object that can be serialized to and from JSON as an array.
/// </summary>
public class PayPalSerializableListObject<T> : List<T>;