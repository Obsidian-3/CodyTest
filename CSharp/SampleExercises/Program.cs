using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleDataManagement.Models;

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
var persons = JsonConvert.DeserializeObject<List<Person>>(File.ReadAllText(personsFilePath));
var organizations = JsonConvert.DeserializeObject<List<Organization>>(File.ReadAllText(organizationsFilePath));
var vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(File.ReadAllText(vehiclesFilePath));
var addresses = JsonConvert.DeserializeObject<List<Address>>(File.ReadAllText(addressesFilePath));

const int SPACES = 10;
string indent = new string(' ', SPACES);


//Test #1: Do all files have entities? (True / False)
bool result1 = true;
if (((persons is null ? 0 : 1) +
     (organizations is null ? 0 : 1) +
     (vehicles is null ? 0 : 1) +
     (addresses is null ? 0 : 1))
    < 4)
     result1 = false;
Console.WriteLine("#1: Do all files have entities?  {0}", result1);
Console.WriteLine();

//Test #2: What is the total count for all entities?
//    Assuming entities are non-null from here - otherwise, the null count above could have been separated out 
//    to its own test and exited the program with an error message if any of them were
Console.WriteLine($"#2: Total entity count = {persons.Count + organizations.Count + vehicles.Count + addresses.Count}");
Console.WriteLine();


//Test #3: What is the count for each type of Entity? Person, Organization, Vehicle, and Address
Console.WriteLine("#3: Individual entity counts:");
Console.WriteLine($"{indent}Person: {persons.Count}");
Console.WriteLine($"{indent}Organization: {organizations.Count}");
Console.WriteLine($"{indent}Vehicle: {vehicles.Count}");
Console.WriteLine($"{indent}Address: {addresses.Count}");
Console.WriteLine();


//Test #4: Provide a breakdown of entities which have associations in the following manor:
//         -Per Entity Count
//         - Total Count
int pCount = persons.Where(x => x.Associations.Count > 0).Select(x => 1).Sum();
int oCount = organizations.Where(x => x.Associations.Count > 0).Select(x => 1).Sum();
int vCount = vehicles.Where(x => x.Associations.Count > 0).Select(x => 1).Sum();
int aCount = addresses.Where(x => x.Associations.Count > 0).Select(x => 1).Sum();
Console.WriteLine("#4: Per entity counts:");
Console.WriteLine($"{indent}Person: {pCount}");
Console.WriteLine($"{indent}Organization: {oCount}");
Console.WriteLine($"{indent}Vehicle: {vCount}");
Console.WriteLine($"{indent}Address: {aCount}");
Console.WriteLine($"{indent}-- Total Count: {pCount + oCount + vCount + aCount}");
Console.WriteLine();


//Test #5: Provide the vehicle detail that is associated to the address "4976 Penelope Via South Franztown, NH 71024"?
//         StreetAddress: "4976 Penelope Via"
//         City: "South Franztown"
//         State: "NH"
//         ZipCode: "71024"

//  Assuming the same address could be associated with multiple vehicles,
//  I'm keeping it simple and just selecting the first vehicle.  Variables a and v
//  are made nullable to avoid overdoing it with the error checking and to keep the 
//  code tidy - we know this case will return a value.

Address? a = addresses.FirstOrDefault(x => (x.StreetAddress == "4976 Penelope Via") && 
                                     (x.City == "South Franztown") && 
                                     (x.State == "NH") && 
                                     (x.ZipCode == "71024"));

Vehicle? v = vehicles.FirstOrDefault(x => x.EntityId == a.Associations[0].EntityId);

/* could have been done in one statement, but not as easy to read:
Vehicle? v = vehicles.FirstOrDefault(x => x.EntityId == addresses.FirstOrDefault(x => (x.StreetAddress == "4976 Penelope Via") &&
                                                                                      (x.City == "South Franztown") &&
                                                                                      (x.State == "NH") &&
                                                                                      (x.ZipCode == "71024")).Associations[0].EntityId);
*/

Console.WriteLine("#5: Vehicle associated with 4976 Penelope Via:");
Console.WriteLine($"{indent}Id: {v.EntityId}");
Console.WriteLine($"{indent}Make: {v.Make}");
Console.WriteLine($"{indent}Model: {v.Model}");
Console.WriteLine($"{indent}Year: {v.Year.ToString()}");
Console.WriteLine($"{indent}Plate: {v.PlateNumber}");
Console.WriteLine($"{indent}State: {v.State}");
Console.WriteLine($"{indent}Vin: {v.Vin}");
Console.WriteLine();


//Test #6: What person(s) are associated to the organization "thiel and sons"?
var org = organizations.FirstOrDefault(x => x.Name.ToLower() == "thiel and sons");
string pResult = "";

if ((org is null) || (org.Associations.Count == 0))
    pResult = "None";
else
{
    var assocList = org.Associations.Where(x => x.EntityType.ToLower() == "vehicle");
    if (assocList is null)
        pResult = "None";
    else
    {
        foreach (Association assoc in assocList)
        {
            Person? p = persons.FirstOrDefault(x => x.EntityId == assoc.EntityId);
            if (p is not null)
                pResult += $"{p.LastName}, {p.FirstName} {p.MiddleName}{Environment.NewLine}";
        }
        if (pResult == "")
            pResult = "None";
    }
}

Console.WriteLine("#6: Which persons are associated with 'Thiel and Sons':");
Console.WriteLine($"{indent}{pResult}");
Console.WriteLine();


//Test #7: How many people have the same first and middle name?
var queryForm =
    from person in persons
    where person.FirstName.ToLower() == person.MiddleName.ToLower()
    select person;

var filterForm = persons.Where(x => x.FirstName.ToLower() == x.MiddleName.ToLower());

Dictionary<string, string> dictForm = new Dictionary<string, string>();
foreach (var person in persons)
    if (person.FirstName.ToLower() == person.MiddleName.ToLower())
        dictForm.Add(person.EntityId, $"{person.FirstName} {person.MiddleName}");

Console.WriteLine("#7: Number of people with the same first and middle name:");
Console.WriteLine($"{indent}By LINQ Query: {queryForm.Count()}");
Console.WriteLine($"{indent}By LINQ Filter: {filterForm.Count()}");
Console.WriteLine($"{indent}By Dictionary: {dictForm.Count()}");
Console.WriteLine();


//Test #8: Provide a breakdown of entities where the EntityId contains "B3" in the following manor:
//         -Total count by type of Entity
//         - Total count of all entities

// taking a break from linq, even though it would be more efficient and less lines than this
Dictionary<string, List<string>> dictB3 = new Dictionary<string, List<string>>();

string searchString = "b3";

foreach (Person person in persons)
{
    if (person.EntityId.ToLower().Contains(searchString))
    {
        if (dictB3.ContainsKey(person.GetType().Name))
            dictB3[person.GetType().Name].Add(person.EntityId);
        else
            dictB3.Add(person.GetType().Name, new List<string> { person.EntityId });
    }
}

foreach (Vehicle vehicle in vehicles)
{
    if (vehicle.EntityId.ToLower().Contains(searchString))
    {
        if (dictB3.ContainsKey(vehicle.GetType().Name))
            dictB3[vehicle.GetType().Name].Add(vehicle.EntityId);
        else
            dictB3.Add(vehicle.GetType().Name, new List<string> { vehicle.EntityId });
    }
}

foreach (Organization org1 in organizations)
{
    if (org1.EntityId.ToLower().Contains(searchString))
    {
        if (dictB3.ContainsKey(org1.GetType().Name))
            dictB3[org1.GetType().Name].Add(org1.EntityId);
        else
            dictB3.Add(org1.GetType().Name, new List<string> { org1.EntityId });
    }
}

foreach (Address address in addresses)
{
    if (address.EntityId.ToLower().Contains(searchString))
    {
        if (dictB3.ContainsKey(address.GetType().Name))
            dictB3[address.GetType().Name].Add(address.EntityId);
        else
            dictB3.Add(address.GetType().Name, new List<string> { address.EntityId });
    }
}

Console.WriteLine("#8: Count of EntityID's containing 'B3' by EntityType:");
int totCount = 0;

foreach (KeyValuePair<string, List<string>> kvp in dictB3)
{
    Console.WriteLine($"{indent}{kvp.Key}: {kvp.Value.Count}");
    totCount += kvp.Value.Count;
}

Console.WriteLine($"{indent}-- Total Count: {totCount}");
Console.WriteLine();



//// #9:    I could stand to see changes to the way the associations are set up.  I'd have to work
//          on it a bit to see if it's doable keeping the current files (I'm thinking it is), but I
//          would rather see everything set up with keys and foreign keys for the associations.  It 
//          would be more complicated when reading the data in, but it would be easier to follow 
//          associations.  I don't know enough yet about EF to know how it does it, but I know it can
//          do most (if not all) of what Django can do.  And in my last job, we would set up foreign
//          keys into other models, and then you could access date in one model from another model using
//          dot-notation with the foreign key.  You could get rid of the EntityType field, but you would
//          then need 4 EntityID field types as the FKs: EntityIdVehicle, EntityIdPerson, etc.  Each table
//          would need association lists of FKs to the other 3 tables:  e.g. Person would have
//              IList<string>? EntityIdVehicle
//              IList<string>? EntityIdOrganization
//              IList<string>? EntityIdAddress
//          However, this gets messier if you add new types of entity models.  You'd have to keep updating
//          the other models with new FKs.  The current method doesn't require that.  So I'm not completely
//          sure which would be better (assuming my change would even be viable).
