IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'PaylocityPayroll')
BEGIN
	CREATE DATABASE PaylocityPayroll
END
GO

BEGIN TRANSACTION
	USE PaylocityPayroll

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Companies')
	BEGIN
		CREATE TABLE Companies
		(
			CompanyId				bigint			NOT NULL IDENTITY(1, 1),
			Name					varchar(100)	NOT NULL,

			CONSTRAINT PK_Companies PRIMARY KEY (CompanyId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Employees')
	BEGIN
		CREATE TABLE Employees
		(
			EmployeeId				bigint			NOT NULL IDENTITY(1, 1),
			CompanyId				bigint			NOT NULL,
			FirstName				varchar(50)		NOT NULL,
			LastName				varchar(50)		NOT NULL,

			CONSTRAINT PK_Employees PRIMARY KEY (EmployeeId),
			CONSTRAINT FK_Employees_Companies FOREIGN KEY (CompanyId)
				REFERENCES Companies(CompanyId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Positions')
	BEGIN
		CREATE TABLE Positions
		(
			PositionId				bigint			NOT NULL IDENTITY(1, 1),
			CompanyId				bigint			NOT NULL,
			Title					varchar(50)		NOT NULL DEFAULT ('Employee'),

			CONSTRAINT PK_Positions PRIMARY KEY (PositionId),
			CONSTRAINT FK_Positions_Companies FOREIGN KEY (CompanyId)
				REFERENCES Companies(CompanyId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EmployeePositions')
	BEGIN
		CREATE TABLE EmployeePositions
		(
			EmployeePositionId		bigint			NOT NULL IDENTITY(1, 1),
			EmployeeId				bigint			NOT NULL,
			PositionId				bigint			NOT NULL,
			PayFrequency			tinyint			NOT NULL DEFAULT (0), -- 0 = N/A, 1 = Daily, 2 = Weekly, 3 = Biweekly, 4 = Semimonthly, 5 = Monthly, 6 = Annually, ...
			PayRate					decimal(19, 4)	NOT NULL DEFAULT (0),

			CONSTRAINT PK_EmployeePositions PRIMARY KEY (EmployeePositionId),
			CONSTRAINT FK_EmployeePositions_Employees FOREIGN KEY (EmployeeId)
				REFERENCES Employees(EmployeeId),
			CONSTRAINT FK_EmployeePositions_Positions FOREIGN KEY (PositionId)
				REFERENCES Positions(PositionId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Benefits')
	BEGIN
		CREATE TABLE Benefits
		(
			BenefitId				bigint			NOT NULL IDENTITY(1, 1),
			CompanyId				bigint			NOT NULL,
			BenefitType				tinyint			NOT NULL DEFAULT (0), -- 0 = N/A, 1 = HealthIns, 2 = DentalIns, 3 = VisionIns, ...
			Description				varchar(50)		NOT NULL,
			AnnualCostEmployee		decimal(19, 4)	NOT NULL DEFAULT (0),
			AnnualCostDependent		decimal(19, 4)	NOT NULL DEFAULT (0),
			EffectiveDate			date			NOT NULL,
			EndDate					date			NULL,

			CONSTRAINT PK_Benefits PRIMARY KEY (BenefitId),
			CONSTRAINT FK_Benefits_Companies FOREIGN KEY (CompanyId)
				REFERENCES Companies(CompanyId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BenefitDiscounts')
	BEGIN
		CREATE TABLE BenefitDiscounts
		(
			BenefitDiscountId		bigint			NOT NULL IDENTITY(1, 1),
			BenefitId				bigint			NOT NULL,
			FilterByColumn			varchar(100)	NOT NULL,
			FilterPattern			nvarchar(4000)	NOT NULL,
			DiscountType			tinyint			NOT NULL DEFAULT (0), -- 0 = N/A, 1 = Percentage, 2 = FlatAmount, 3 = RateOverride, ...
			DiscountAmount			decimal(19, 4)	NOT NULL DEFAULT (0),

			CONSTRAINT PK_BenefitDiscounts PRIMARY KEY (BenefitDiscountId),
			CONSTRAINT FK_BenefitDiscounts_Benefits FOREIGN KEY (BenefitId)
				REFERENCES Benefits(BenefitId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EmployeeEnrollments')
	BEGIN
		CREATE TABLE EmployeeEnrollments
		(
			EmployeeEnrollmentId	bigint			NOT NULL IDENTITY(1, 1),
			EmployeeId				bigint			NOT NULL,
			EnrollmentEventType		tinyint			NOT NULL DEFAULT (0), -- 0 = N/A, 1 = NewHire, 2 = OpenEnroll, 3 = CoverageLoss, 4 = HouseholdChange, ...
			EffectiveDate			date			NOT NULL,
			EndDate					date			NULL,

			CONSTRAINT PK_EmployeeEnrollments PRIMARY KEY (EmployeeEnrollmentId),
			CONSTRAINT FK_EmployeeEnrollments_Employees FOREIGN KEY (EmployeeId)
				REFERENCES Employees(EmployeeId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EnrollmentDependents')
	BEGIN
		CREATE TABLE EnrollmentDependents
		(
			EnrollmentDependentId	bigint			NOT NULL IDENTITY(1, 1),
			EmployeeEnrollmentId	bigint			NOT NULL,
			FirstName				varchar(50)		NOT NULL,
			LastName				varchar(50)		NOT NULL,

			CONSTRAINT PK_EnrollmentDependents PRIMARY KEY (EnrollmentDependentId),
			CONSTRAINT FK_EnrollmentDependents_EmployeeEnrollments FOREIGN KEY (EmployeeEnrollmentId)
				REFERENCES EmployeeEnrollments(EmployeeEnrollmentId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'EnrollmentBenefits')
	BEGIN
		CREATE TABLE EnrollmentBenefits
		(
			EnrollmentBenefitId		bigint			NOT NULL IDENTITY(1, 1),
			EmployeeEnrollmentId	bigint			NOT NULL,
			BenefitId				bigint			NOT NULL,

			CONSTRAINT PK_EnrollmentBenefits PRIMARY KEY (EnrollmentBenefitId),
			CONSTRAINT FK_EnrollmentBenefits_EmployeeEnrollments FOREIGN KEY (EmployeeEnrollmentId)
				REFERENCES EmployeeEnrollments(EmployeeEnrollmentId),
			CONSTRAINT FK_EnrollmentBenefits_Benefits FOREIGN KEY (BenefitId)
				REFERENCES Benefits(BenefitId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PayRuns')
	BEGIN
		CREATE TABLE PayRuns
		(
			PayRunId				bigint			NOT NULL IDENTITY(1, 1),
			CompanyId				bigint			NOT NULL,
			PayRunType				tinyint			NOT NULL DEFAULT (0), -- 0 = Regular, 1 = Adjustment, 2 = Bonus, ...
			PayRunStatus			tinyint			NOT NULL DEFAULT (0), -- 0 = Created, 1 = Calculating, 2 = Failed, 3 = Ready, 4 = Posted, 5 = Voided, ...
			EarningsTotal			decimal(19, 4)	NOT NULL DEFAULT (0),
			DeductionsTotal			decimal(19, 4)	NOT NULL DEFAULT (0),
			WithholdingsTotal		decimal(19, 4)	NOT NULL DEFAULT (0),
			NetTotal				decimal(19, 4)	NOT NULL DEFAULT (0),
			PayPeriodFrom			date			NOT NULL,
			PayPeriodTo				date			NOT NULL,
			PayDate					date			NOT NULL,

			CONSTRAINT PK_PayRuns PRIMARY KEY (PayRunId),
			CONSTRAINT FK_PayRuns_Companies FOREIGN KEY (CompanyId)
				REFERENCES Companies(CompanyId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PayRunEmployees')
	BEGIN
		CREATE TABLE PayRunEmployees
		(
			PayRunEmployeeId		bigint			NOT NULL IDENTITY(1, 1),
			PayRunId				bigint			NOT NULL,
			EmployeeId				bigint			NOT NULL,
			EarningsAmount			decimal(19, 4)	NOT NULL DEFAULT (0),
			DeductionsAmount		decimal(19, 4)	NOT NULL DEFAULT (0),
			WithholdingsAmount		decimal(19, 4)	NOT NULL DEFAULT (0),
			NetAmount				decimal(19, 4)	NOT NULL DEFAULT (0),

			CONSTRAINT PK_PayRunEmployees PRIMARY KEY (PayRunEmployeeId),
			CONSTRAINT FK_PayRunEmployees_PayRuns FOREIGN KEY (PayRunId)
				REFERENCES PayRuns(PayRunId),
			CONSTRAINT FK_PayRunEmployees_Employees FOREIGN KEY (EmployeeId)
				REFERENCES Employees(EmployeeId)
		)
	END
	GO

	IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PayRunDetails')
	BEGIN
		CREATE TABLE PayRunDetails
		(
			PayRunDetailId			bigint			NOT NULL IDENTITY(1, 1),
			PayRunEmployeeId		bigint			NOT NULL,
			PayRunDetailType		tinyint			NOT NULL DEFAULT (0), -- 0 = N/A, 1 = Earning, 2 = Deduction, 3 = Withholding, ...
			Amount					decimal(19, 4)	NOT NULL DEFAULT (0),

			CONSTRAINT PK_PayRunDetails PRIMARY KEY (PayRunDetailId),
			CONSTRAINT FK_PayRunDetails_PayRunEmployees FOREIGN KEY (PayRunEmployeeId)
				REFERENCES PayRunEmployees(PayRunEmployeeId)
		)
	END
	GO

COMMIT TRANSACTION