using System;

namespace FIS.Records
{
    /// <summary>
    /// Represents a Single Faculty Record.
    /// </summary>
    internal struct Faculty
    {
        /// <summary>
        /// Personal record of the faculty.
        /// </summary>
        internal PersonalRec PersonalRec { get; private set; }

        /// <summary>
        /// Unique Identification of the faculty.
        /// </summary>
        internal int ID { get; private set; }

        /// <summary>
        /// Date hired of the faculty.
        /// </summary>
        internal DateTime DateHired { get; private set; }

        /// <summary>
        /// The current status of the faculty.
        /// </summary>
        internal string Status { get; private set; }

        Faculty (int ID, PersonalRec PersonalRec, DateTime DateHired, string Status)
        {
            this.ID = ID;
            this.PersonalRec = PersonalRec;
            this.DateHired = DateHired;
            this.Status = Status;
        }

        internal static Faculty Create (int ID, PersonalRec PersonalRec, DateTime DateHired, string Status) =>
            new Faculty(ID: ID, PersonalRec: PersonalRec, DateHired: DateHired, Status: Status);
    }
}