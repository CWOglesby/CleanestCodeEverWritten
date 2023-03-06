BEGIN TRANSACTION

	USE PaylocityPayroll

	INSERT INTO Companies
		(Name)
	VALUES
	(
		'Acme, Inc.'
	)

	INSERT INTO Positions
		(CompanyId, Title)
	VALUES
	(
		(SELECT TOP 1 CompanyId FROM Companies),
		'Employee'
	)

	INSERT INTO Benefits
		(CompanyId, BenefitType, Description, AnnualCostEmployee, AnnualCostDependent, EffectiveDate)
	VALUES
	(
		(SELECT TOP 1 CompanyId FROM Companies),
		1, -- 1 = HealthInsurance
		'Health2023',
		1000.00,
		500.00,
		'2023-01-01'
	)

	INSERT INTO Employees
		(CompanyId, FirstName, LastName)
	VALUES
	(
		(SELECT TOP 1 CompanyId FROM Companies),
		'Bob',
		'Jones'
	),
	(
		(SELECT TOP 1 CompanyId FROM Companies),
		'Alex',
		'Smith'
	),
	(
		(SELECT TOP 1 CompanyId FROM Companies),
		'John',
		'Brown'
	)

	INSERT INTO EmployeePositions
		(EmployeeId, PositionId, PayFrequency, PayRate)
	SELECT
		EmployeeId,
		(SELECT TOP 1 PositionId FROM Positions),
		3, -- 3 = Biweekly
		2000.00
	FROM Employees

	INSERT INTO EmployeeEnrollments
		(EmployeeId, EnrollmentEventType, EffectiveDate)
	SELECT
		EmployeeId,
		2, -- 2 = OpenEnroll
		'2023-01-01'
	FROM Employees

	INSERT INTO EnrollmentBenefits
		(EmployeeEnrollmentId, BenefitId)
	SELECT
		EmployeeEnrollmentId,
		(SELECT TOP 1 BenefitId FROM Benefits)
	FROM EmployeeEnrollments

	INSERT INTO EnrollmentDependents
		(EmployeeEnrollmentId, FirstName, LastName)
	SELECT
		ee.EmployeeEnrollmentId,
		v.valueId,
		e.LastName
	FROM EmployeeEnrollments ee
	INNER JOIN Employees e
		ON ee.EmployeeId = e.EmployeeId
	INNER JOIN (VALUES ('David'), ('Ashley'), ('Joseph')) v(valueId)
		ON 1 = 1

COMMIT TRANSACTION