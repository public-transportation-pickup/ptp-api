## Note for CLI, viết quài mợt 
dotnet ef migrations remove  -s .\APIs\PTP.WebAPI\ -p .\APIs\PTP.Infrastructure\
dotnet ef migrations add V2_01_ChangeDbSchema -s .\APIs\PTP.WebAPI\ -p .\APIs\PTP.Infrastructure\
dotnet ef database update -s .\APIs\PTP.WebAPI\ -p .\APIs\PTP.Infrastructure\
dotnet ef database drop -s .\APIs\PTP.WebAPI\ -p .\APIs\PTP.Infrastructure\
dotnet run --project .\APIs\PTP.WebAPI\