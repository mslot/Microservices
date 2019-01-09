# Microservices

A test repository for building microservices deployed through Microsoft DevOps.

# General
This is my way of trying out:

1. deployment of small services spread out to Azure Functions, and web apis. Does DevOps support deployment of small code changes and will only the resources that has changed, be deployed (I dont want to stop all systems when a small part of code changes in one resource)
2. how well integrated is the Key Vault into web api, azure functions and ARM template? How much code should it take for me to put connection strings and secrets into vault and to retrieve it?
3. How does GitFlow fit with nuget production? How is the feed integrated, and what features does it contain.
4. What is this YAML build thing? (;))

This project only contains some functions and a webapi but I want to test to see how and what DevOps was capable of. 

## Deployment of small services
I was capable of producing several small artifacts (one for web api and one for my azure function albeit this can be scaled to many api and many funcions. I found out that a deploy with MsDeploy activated in the release and the use of "-useChecksum" only will deploy code dlls and config files that has changed. This is an important "discovery", because at first sight this also happens when you do it with out msdeploy and checksum, because only the date and times of the changed files produced will change (i think that the agents is somehow reused ... I THINK). This will therefore not change the date and time for all dlls produced, and therefore only those with different timestamps will be deployed. This will break over time. Therefore use MsDeploy and -useChecksum, of you can. The only thing that wasn't capable of this was when i deployed to a slot on the webapi, other than the production slot. When I did this, the slot never reloaded, even though the updated dlls was deployed. I had to add an extra task after the deploy to the slot, that restarted the resource. 

My recommandation is: use deployment slots and use msdeploy with checksum flagated.

## KeyVault
Wow. What a feature. It took me quite some time to do the plumming, but it works for web apis, ARM templates, scripts and azure functions. It has some quirks, but with system integreated users being built into the ARM deployment, we can grant reources access to the vault. With the update to Azure functions in august, we are maybe on the right path towards not having to store our secrets in the environment variables that is shown on the settings page of the different resources.

Note: not all resources yet fully integrate with having access to the vault with a simple system assigned user (that is added from the ARM template). 

# NuGet
Gitflow and the feed of devops fully supports the use of prereleases and releases. What is yet to be fixed is the release part of the nuget. I want to be able to take a nuget package fra a prerelease state to a released state without recompiling the code.

Note: in this project i have put the nuget code with the "main" project. This is of course not the right pattern. You want to have a seperate repository only containing the code that is going to packaged so tags, and branching doesnt interfer with the "main" project (if you are producing nugets that is going to be used in one of the microservices).


