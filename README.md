#FunBooksAndVideos e-commerce shop

##Introduction

FunBooksAndVideos is an e-commerce shop where customers can view books and watch online videos. Users can have memberships for the book club, the video club or for both clubs (premium).

##Purchase Order

A purchase order can contain products or membership requests. A purchase order has an PO ID, a customer ID and total price. There is an item line in the purchase order per product purchased (product, membership type). One example of a purchase order is the following:

Purchase Order: 3344656 Total: 48.50 Customer: 4567890 Item lines:

· Video "Comprehensive First Aid Training"

· Book "The Girl on the train"

· Book Club Membership

##Business Rules

Several business rules are applied when a purchase order is processed. Some of the business rules are shown in this list:

· BR1. If the purchase order contains a membership, it has to be activated in the customer account immediately.

· BR2. If the purchase order contains a physical product a shipping slip has to be generated.

##Tasks

· Implement an Object Oriented model of the system

· Design a flexible purchase order processor

· Implement the above business rules

## Project Details

Tools Used - .Net Framework 4.7, Visual Studio 2017, UnitTest - Nunit, Mocking - Moq
------------------------------------------------------------------------------------
The project contains a class library for processing purchase order from FunBookAndVideos e-commerce shop.
Test project contains the unit test to cover all the business rules in the problem statement.
Tried to make it simple and readable. 
As it is more of design exercise, didn't cover integration test or acceptance test which we can cover with specflow.
------------------------------------------------------------------------------------------
PurchaseOrderService is the processor to process all the incoming orders which has constructor injection for BusinessRuleProvider, Which provides the relevant businessrules for a particular product type.
Once we have the list of rule it applies the rules and stores the data in the relevant repository.
-------------------------------------------------------------------------------------------