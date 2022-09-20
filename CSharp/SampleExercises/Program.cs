var dataSourcesDirectory = Path.Combine(Environment.CurrentDirectory, "DataSources");
var personsFilePath = Path.Combine(dataSourcesDirectory, "Persons_20220824_00.json");
var organizationsFilePath = Path.Combine(dataSourcesDirectory, "Organizations_20220824_00.json");
var vehiclesFilePath = Path.Combine(dataSourcesDirectory, "Vehicles_20220824_00.json");
var addressesFilePath = Path.Combine(dataSourcesDirectory, "Addresses_20220824_00.json");

//Quick test to ensure that all files are where they should be :)
foreach (var path in new[] { personsFilePath, organizationsFilePath, vehiclesFilePath, addressesFilePath })
{
    if (!File.Exists(path))
        throw new FileNotFoundException(path);
}

//TODO: Start your exercise here. Do not forget about answering Test #9 (Handled slightly different)
// Reminder: Collect the data from each file. Hint: You could use Newtonsoft's JsonConvert or Microsoft's JsonSerializer
throw new NotImplementedException("Get data from file");

//Test #1: Do all files have entities? (True / False)
throw new NotImplementedException("Complete Test #1");

//Test #2: What is the total count for all entities?
throw new NotImplementedException("Complete Test #2");

//Test #3: What is the count for each type of Entity? Person, Organization, Vehicle, and Address
throw new NotImplementedException("Complete Test #3");

//Test #4: Provide a breakdown of entities which have associations in the following manor:
//         -Per Entity Count
//         - Total Count
throw new NotImplementedException("Complete Test #4");

//Test #5: Provide the vehicle detail that is associated to the address "4976 Penelope Via South Franztown, NH 71024"?
throw new NotImplementedException("Complete Test #5");

//Test #6: What person(s) are associated to the organization "Thiel and Sons"?
throw new NotImplementedException("Complete Test #6");

//Test #7: How many people have the same first and middle name?
throw new NotImplementedException("Complete Test #7");

//Test #8: Provide a breakdown of entities where the EntityId contains "B3" in the following manor:
//         -Total count by type of Entity
//         - Total count of all entities
throw new NotImplementedException("Complete Test #8");