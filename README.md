# OurWater


Used DB Schema: V2


1. Desktop (admin)
    - Login
    - View Consumption Debit Records
        - Review Consumption Debit Record
    - View Production Debit Records
    - Submit Production Debit Record
    - View Possible Pipe Leak
    - View Customer Bills
        - Review Customer Payment
    - Manage Users (CRUD)
    - Setting Fines
2. Mobile (officer/customer)
    - Login
    - Submit Consumption Debit Record
    - View Consumption Debit Record
    - `(Officer)` Review Consumption Debit Record
    - `(Customer)` View Charged Bill
    - `(Customer)` Pay Charged Bill
3. API
    - Login
    - Get all consumption debit record
    - Get consumption debit record detail by id
    - Get consumption debit record detail for this month (opt)
    - Submit consumption debit record
    - Update consumption debit record (officer)
    - Get customer bills (customer)
    - Submit customer payment (customer)