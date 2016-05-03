using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typeform_Attendance
{
    class Person
    {
        private string firstName;
        private string lastName;
        private string email;
        private int meeting;

        public int Meeting
        {
            get
            {
                return meeting;
            }

            set
            {
                meeting = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                lastName = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public Person(string f, string l, string e)
        {
            firstName = f;
            lastName = l;
            email = e;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Person p = (Person)obj;
                return firstName == p.FirstName && lastName == p.LastName && email == p.Email && meeting == p.Meeting;
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }
        }

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + firstName.GetHashCode();
            hash = (hash * 7) + lastName.GetHashCode();
            hash = (hash * 7) + meeting.GetHashCode();
            hash = (hash * 7) + email.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return "First Name: " + firstName + " Last Name: " + lastName + " Attendance: " + meeting;
        }
    }
}
