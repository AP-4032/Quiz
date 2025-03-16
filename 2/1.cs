using System;

public class Student
{
    public int StudentId { get; private set; }
    public string Name { get; private set; }
    private int[] grades;

    public Student(int studentId, string name)
    {
        StudentId = studentId;
        Name = name;
        grades = new int[5]; // Initialize grades with 5 zeros
    }

    public void SetGrade(int courseIndex, int grade)
    {
        if (courseIndex < 0 || courseIndex > 4)
        {
            Console.WriteLine("Error: outOfRange");
            return;
        }

        if (grade < 0 || grade > 20)
        {
            Console.WriteLine("Error: outOfRange");
            return;
        }

        grades[courseIndex] = grade;
    }

    public double CalculateAverage()
    {
        int sum = 0;
        foreach (int grade in grades)
        {
            sum += grade;
        }
        return (double)sum / grades.Length;
    }

    public void DisplayStudentInfo()
    {
        Console.Write($"Student ID: {StudentId}, Name: {Name}, Grades: ");
        foreach (int grade in grades)
        {
            Console.Write(grade + " ");
        }
        Console.WriteLine();
    }
}

public class Classroom
{
    private Student[] students;
    private int studentCount;

    public Classroom()
    {
        students = new Student[30];
        studentCount = 0;
    }

    public void AddStudent(Student newStudent)
    {
        if (studentCount >= 30)
        {
            Console.WriteLine("Error: Classroom is full");
            return;
        }

        for (int i = 0; i < studentCount; i++)
        {
            if (students[i].StudentId == newStudent.StudentId)
            {
                Console.WriteLine("Error: repetitive studentId");
                return;
            }
        }

        students[studentCount] = newStudent;
        studentCount++;
    }

    public void RemoveStudentById(int studentId)
    {
        for (int i = 0; i < studentCount; i++)
        {
            if (students[i].StudentId == studentId)
            {
                // Shift elements to fill the gap
                for (int j = i; j < studentCount - 1; j++)
                {
                    students[j] = students[j + 1];
                }
                students[studentCount - 1] = null;
                studentCount--;
                Console.WriteLine($"Student with ID {studentId} removed successfully");
                return;
            }
        }
        Console.WriteLine($"Error: Student with ID {studentId} not found");
    }

    public void FindStudentById(int studentId)
    {
        for (int i = 0; i < studentCount; i++)
        {
            if (students[i].StudentId == studentId)
            {
                students[i].DisplayStudentInfo();
                return;
            }
        }
        Console.WriteLine($"Error: Student with ID {studentId} not found");
    }

    public double CalculateClassAverage()
    {
        if (studentCount == 0) return 0.0;

        double total = 0.0;
        for (int i = 0; i < studentCount; i++)
        {
            total += students[i].CalculateAverage();
        }
        return total / studentCount;
    }

    public void DisplayAllStudents()
    {
        if (studentCount == 0)
        {
            Console.WriteLine("No students in the classroom");
            return;
        }

        for (int i = 0; i < studentCount; i++)
        {
            students[i].DisplayStudentInfo();
        }
    }

    public void DisplayRanking()
    {
        if (studentCount == 0)
        {
            Console.WriteLine("هیچ دانشجویی در کلاس وجود ندارد");
            return;
        }

        // ایجاد یک آرایه موقت برای نگهداری دانشجویان و میانگین‌ها
        Student[] tempStudents = new Student[studentCount];
        double[] averages = new double[studentCount];

        // کپی دانشجویان و محاسبه میانگین‌ها
        for (int i = 0; i < studentCount; i++)
        {
            tempStudents[i] = students[i];
            averages[i] = students[i].CalculateAverage();
        }

        // مرتب‌سازی دانشجویان بر اساس میانگین (نزولی) و شماره دانشجویی (صعودی)
        for (int i = 0; i < studentCount - 1; i++)
        {
            for (int j = i + 1; j < studentCount; j++)
            {
                if (averages[i] < averages[j] || 
                    (averages[i] == averages[j] && tempStudents[i].StudentId > tempStudents[j].StudentId))
                {
                    // جابجایی میانگین‌ها
                    double tempAvg = averages[i];
                    averages[i] = averages[j];
                    averages[j] = tempAvg;

                    // جابجایی دانشجویان
                    Student tempStudent = tempStudents[i];
                    tempStudents[i] = tempStudents[j];
                    tempStudents[j] = tempStudent;
                }
            }
        }

        // نمایش رتبه‌بندی
        Console.WriteLine("\nرتبه‌بندی دانشجویان:");
        for (int i = 0; i < studentCount; i++)
        {
            Console.WriteLine($"{i + 1}. {tempStudents[i].StudentId} - {tempStudents[i].Name} - {averages[i]:0.00}");
        }
    }
}

public class MainClass
{
    public static void Main(string[] args)
    {
        // Create a classroom
        Classroom classroom = new Classroom();

        // Add students
        Student student1 = new Student(101, "Ali");
        student1.SetGrade(0, 18);
        student1.SetGrade(1, 15);
        student1.SetGrade(2, 20);
        student1.SetGrade(3, 17);
        student1.SetGrade(4, 19);
        classroom.AddStudent(student1);

        Student student2 = new Student(102, "Neda");
        student2.SetGrade(0, 19);
        student2.SetGrade(1, 20);
        student2.SetGrade(2, 18);
        student2.SetGrade(3, 16);
        student2.SetGrade(4, 20);
        classroom.AddStudent(student2);

        Student student3 = new Student(103, "Reza");
        student3.SetGrade(0, 15);
        student3.SetGrade(1, 17);
        student3.SetGrade(2, 18);
        student3.SetGrade(3, 16);
        student3.SetGrade(4, 19);
        classroom.AddStudent(student3);

        // Test methods
        Console.WriteLine("\nAll Students:");
        classroom.DisplayAllStudents();

        Console.WriteLine("\nClass Average:");
        Console.WriteLine(classroom.CalculateClassAverage().ToString("0.00"));

        Console.WriteLine("\nFinding Student by ID (102):");
        classroom.FindStudentById(102);

        Console.WriteLine("\nRemoving Student by ID (101):");
        classroom.RemoveStudentById(101);

        Console.WriteLine("\nAll Students after removal:");
        classroom.DisplayAllStudents();

        Console.WriteLine("\nDisplaying Student Ranking:");
        classroom.DisplayRanking();
    }
}