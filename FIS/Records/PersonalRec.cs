using System;

namespace FIS.Records
{
  struct PersonalRec
  {
    internal string FirstName { get; private set; }
    internal string MiddleName { get; private set; }
    internal string LastName { get; private set; }
    internal int Age { get; private set; }
    internal string Address { get; private set; }
    internal int CellPhoneNo { get; private set; }
    internal DateTime DateOfBirth { get; private set; }
    internal string PlaceOfBirth { get; private set; }

    PersonalRec(string firstName, string middleName, string lastName, int age, string address, int cellPhoneNo, DateTime dateOfBirth, string placeOfBirth)
    {
      FirstName = firstName; MiddleName = middleName;
      LastName = lastName; Age = age;
      Address = address; CellPhoneNo = cellPhoneNo;
      DateOfBirth = dateOfBirth; PlaceOfBirth = placeOfBirth;
    }

    internal static PersonalRec Create(string FirstName, string MiddleName, string LastName, int Age, string Address, int CellPhoneNo, DateTime DateOfBirth, string PlaceOfBirth) => new PersonalRec(firstName: FirstName, middleName: MiddleName, lastName: LastName, age: Age, address: Address, cellPhoneNo: CellPhoneNo, dateOfBirth: DateOfBirth, placeOfBirth: PlaceOfBirth);

    internal PersonalRec withFirstName(string FirstName)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withMiddleName(string MiddleName)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withLastName(string LastName)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withAge(int Age)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withAddress(string Address)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withCellphoneNo(int CellPhoneNo)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withDateOfBirth(DateTime DateOfBirth)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    internal PersonalRec withPlaceOfBirth(string PlaceOfBirth)
      => Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, CellPhoneNo: CellPhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
  }
}