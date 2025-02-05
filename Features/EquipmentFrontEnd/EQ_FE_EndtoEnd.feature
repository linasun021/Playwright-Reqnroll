Feature: EQ-FE-EndtoEnd
  As a customer
  I want to browse, select, and purchase equipment
  So that I can receive the items I need

@store
Scenario Outline: Checkout as a Guest - Single Item
    Given I am located on home page of equipment store
    When I navigate to Equipment Page via Shop all
    And I add <singleItemName> to cart on Equipment page
    Then I should see equipment <singleItemName> be added into Cart Dialogs
    And  I should see the correct price and Subtotal in Cart Dialogs
    When I click CHECKOUT button on Cart Dialogs
    And I enter guest customer <customerName> info on Shipping Page
    Then I should see the Order Total include the calculated Tax on Shipping Page 
    And I should see free shipping is displayed on Shipping Page
    When I click Continue to payment on Shipping Page
    And I enter <paymentOptions> payment info of <customerName> on Payment Page
    Then I should see order success message Congratulations on your order! on Order Success page
    And I should see the order number is presented on Order Success page
    And I should see the Create your account here link on Order Success page
    And I should see the email address of <customerName> is displayed on OrderSuccess page
    And I should see the order confirm email has been send to customer <customerName> with correct info

Examples: 
   | singleItemName                          | customerName | paymentOptions |
  # | LES MILLS SMARTBAR™ Single Bar          | Customer01   | credit card    |
   #| LES MILLS Yoga Mat - Grey               | Customer03   | PayPal         |
   | SMARTBAR™ 2.2 lbs WEIGHT PLATE (SINGLE) | Customer01   | credit card    |

#@store
#Scenario Outline: Checkout as an Existing Customer - Bundle Item
#    Given I am located on home page of equipment store
#    When I navigate to Equipment Page via Workout bundles
#    When I click equipment <bundleItemName> on Equipment page
#    And  I click Add to cart on Product Details page
#    Then I should see equipment <bundleItemName> be added into Cart Dialogs
#    And  I should see free subscription FREE -  LES MILLS+ - 12 Months Subscription be added into Cart Dialogs
#    And  I should see the correct price and Subtotal in Cart Dialogs
#    When I click CHECKOUT button on Cart Dialogs
#    And I close the Free Gift Popup
#    When I enter existing customer <customerName> info on Shipping Page
#    Then I should see the shipping info be entered automaticlly
#    When I enter discount code <disCountName>
#    Then I should see <discountValue>% discount is applied
#    And I should see the Order Total include the calculated Tax and exclude the discount on Shipping Page
#    And I should see free shipping is displayed on Shipping Page
#    When I click Continue to payment on Shipping Page
#    And I enter <paymentOptions> payment info of Customer02 on Payment Page
#    Then I should see order success message Congratulations on your order! on Order Success page
#    And I should see the order number is presented on Order Success page
#    And I should see the email address of <customerName> is displayed on OrderSuccess page 
#    And I should see the order confirm email has been send to customer <customerName> with correct info
#
#Examples: 
#   | bundleItemName | customerName | disCountName   | discountValue | paymentOptions |
#   #| BARBELL SET    | Customer02   | TestAutomation | 20            | PayPal         |
#   | POWER UP       | Customer04   | TestAutomation | 20            | credit card    |
#   
#
#
#@store
#Scenario Outline: Checkout as a New register Customer - Bundle Item
#    Given I am located on home page of equipment store
#    When I create a new customer account Customerxx
#    And I active my new account
#    And I login using this new account
#    And I add shipping address information
#    When I navigate to Equipment Page via Workout bundles
#    When I click equipment <bundleItemName> on Equipment page
#    And  I click Add to cart on Product Details page
#    Then I should see equipment <bundleItemName> be added into Cart Dialogs
#    And  I should see free subscription FREE -  LES MILLS+ - 12 Months Subscription be added into Cart Dialogs
#    And  I should see the correct price and Subtotal in Cart Dialogs
#    When I click CHECKOUT button on Cart Dialogs
#    And I close the Free Gift Popup
#    Then I should see the shipping info be entered automaticlly
#    When I enter discount code <disCountName>
#    Then I should see <discountValue>% discount is applied
#    And I should see the Order Total include the calculated Tax and exclude the discount on Shipping Page
#    And I should see free shipping is displayed on Shipping Page
#    When I click Continue to payment on Shipping Page
#    And I enter <paymentOptions> payment info of Customerxx on Payment Page
#    Then I should see order success message Congratulations on your order! on Order Success page
#    And I should see the order number is presented on Order Success page
#    And I should see the email address of Customerxx is displayed on OrderSuccess page 
#    And I should see the order confirm email has been send to customer Customerxx with correct info
#    When I click Active now in customer email for LesMILLs+Account
#    Then I should see the LesMILLs+Account register page be opened
#Examples: 
#    | bundleItemName | disCountName   | discountValue | paymentOptions |
#    | BARBELL SET    | TestAutomation | 20            | PayPal         |
#    #| POWER UP       | TestAutomation | 20            | credit card    |


