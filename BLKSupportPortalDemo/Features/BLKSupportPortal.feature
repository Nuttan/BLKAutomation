Feature: BLK Support Portal

@smoke
Scenario: Login
	Given I have navigated to the application
	And I see application opened
	When I click "login" link
	When I enter UserName and Password
	| UserName | Password |
	| Nuttan.swain@hexagon.com    | HyundaiCreta@2024# |
	Then I click "login" button
	Then I should see the username with hello
	Then I click "provisioning" link
	And I have navigated to provisioning page
	And I select ProvisioningStatus and GroupName
	| ProvisioningStatus | GroupName |
	| REGISTERED    | BLKCloud-QA |
	And I click "search" button
	Then I click logout