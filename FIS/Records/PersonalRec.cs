﻿using System;

namespace FIS.Records
{
    /// <summary>
    /// Represents A Single Personal Record.
    /// </summary>
    struct PersonalRec
    {
        internal string FirstName { get; private set; }
        internal string MiddleName { get; private set; }
        internal string LastName { get; private set; }
        internal int Age { get; private set; }
        internal string Address { get; private set; }
        internal string PhoneNo { get; private set; }
        internal DateTime DateOfBirth { get; private set; }
        internal string PlaceOfBirth { get; private set; }

        PersonalRec (string FirstName, string MiddleName, string LastName, int Age, string Address, string PhoneNo, DateTime DateOfBirth, string PlaceOfBirth)
        {
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.Age = Age;
            this.Address = Address;
            this.PhoneNo = PhoneNo;
            this.DateOfBirth = DateOfBirth;
            this.PlaceOfBirth = PlaceOfBirth;
        }

        /// <summary>
        /// Creates A New Single Personal Record.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="MiddleName"></param>
        /// <param name="LastName"></param>
        /// <param name="Age"></param>
        /// <param name="Address"></param>
        /// <param name="PhoneNo"></param>
        /// <param name="DateOfBirth"></param>
        /// <param name="PlaceOfBirth"></param>
        /// <returns>New Single Instance of Personal Record.</returns>
        internal static PersonalRec Create (string FirstName, string MiddleName, string LastName, int Age, string Address, string PhoneNo, DateTime DateOfBirth, string PlaceOfBirth) =>
            new PersonalRec(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given First Name.
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns></returns>
        internal PersonalRec withFirstName (string FirstName) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
        
        /// <summary>
        /// Creates A New Single Personal Record with the given Middle Name.
        /// </summary>
        /// <param name="MiddleName"></param>
        /// <returns></returns>
        internal PersonalRec withMiddleName (string MiddleName) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Last Name.
        /// </summary>
        /// <param name="LastName"></param>
        /// <returns></returns>
        internal PersonalRec withLastName (string LastName) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Age.
        /// </summary>
        /// <param name="Age"></param>
        /// <returns></returns>
        internal PersonalRec withAge (int Age) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Address.
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        internal PersonalRec withAddress (string Address) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Phone No.
        /// </summary>
        /// <param name="PhoneNo"></param>
        /// <returns></returns>
        internal PersonalRec withPhoneNo (string PhoneNo) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Date of Birth.
        /// </summary>
        /// <param name="DateOfBirth"></param>
        /// <returns></returns>
        internal PersonalRec withDateOfBirth (DateTime DateOfBirth) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);

        /// <summary>
        /// Creates A New Single Personal Record with the given Place of Birth.
        /// </summary>
        /// <param name="PlaceOfBirth"></param>
        /// <returns></returns>
        internal PersonalRec withPlaceOfBirth (string PlaceOfBirth) =>
            Create(FirstName: FirstName, MiddleName: MiddleName, LastName: LastName, Age: Age, Address: Address, PhoneNo: PhoneNo, DateOfBirth: DateOfBirth, PlaceOfBirth: PlaceOfBirth);
    }
}