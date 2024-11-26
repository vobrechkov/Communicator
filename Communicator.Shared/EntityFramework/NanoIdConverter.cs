using Communicator.Shared.Entity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Communicator.Shared.EntityFramework;

public class NanoIdConverter() : ValueConverter<NanoId, string>(nanoId => nanoId.Value, value => new NanoId(value));