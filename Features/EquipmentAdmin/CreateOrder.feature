Feature: CreateOrder
As an Administrator
I want the ability to create orders
So that I can efficiently manage and fulfill requests as needed
@admin
Scenario Outline:Place an order via Admin
    Given I am located on equipment admin using <adminName>
    When I create New Order on product <productName> for existing customer <customerName> in store <storeName>
    Then I should see the success message You created the order.
    And I should see the admin created order confirm email has been send to customer <customerName> with correct info
Examples: 
    | productName           | adminName | customerName | storeName                       |
    | Vertical Storage Rack | Admin01   | Customer02   | US Equipment Store View - Clubs |

    
