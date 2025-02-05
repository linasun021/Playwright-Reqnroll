#Feature: APIAdminStoreIntegration
#As an integration tester
#
#I need to validate the integration of the Admin, Store and API
#
#So that I can ensure reliable and accurate communication between the store's backend services and other system components
#
#@store @api @admin
#Scenario Outline: Checkout and Ship the order - Guest Customer - Single Item
#	Given I am located on home page of equipment store
#    When I navigate to Equipment Page via Shop all
#    And I add <singleItemName> to cart on Equipment page
#    Then I should see equipment <singleItemName> be added into Cart Dialogs
#    And  I should see the correct price and Subtotal in Cart Dialogs
#    When I click CHECKOUT button on Cart Dialogs
#    And I enter guest customer <customerName> info on Shipping Page
#    Then I should see the Order Total include the calculated Tax on Shipping Page 
#    And I should see free shipping is displayed on Shipping Page
#    When I click Continue to payment on Shipping Page
#    And I enter <paymentOptions> payment info of <customerName> on Payment Page
#    Then I should see order success message Congratulations on your order! on Order Success page
#    And I should see the order number is presented on Order Success page
#    And I should see the Create your account here link on Order Success page
#    And I should see the email address of <customerName> is displayed on OrderSuccess page
#    And I should see the order confirm email has been send to customer <customerName> with correct info
#    When I am located on equipment admin using Admin01
#    Then I should see the order status is Processing on equipment admin
#    When I ship the order using MF API
#    Then I should see the status code is 200
#    And I should see the order status is Complete on equipment admin
#    And I should see the shipped confirm email has beed send to customer <customerName> with correct info
#
#Examples: 
#   | singleItemName                 | customerName | paymentOptions |
#   #| LES MILLS SMARTBAR™ Single Bar | Customer01   | credit card    |
#   | LES MILLS Yoga Mat - Grey      | Customer03   | credit card    |
