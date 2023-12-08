# M Core API
A solution which creates a structure and a framwork for API oriented resposibilities.
Solution currently serves purpose as a job candidate test, where core responsibilities were to create:
- Watch API - that can calculate least angle of watch hands
- Inventory Management System - API that orchestrate inventory items CRUD operations

# Architecutre 

[<img width="378" alt="image" src="https://github.com/Dominosam/m-core-api/assets/46669257/dd8a226c-3777-427a-8b62-cd7668b92287">](https://viewer.diagrams.net/?tags=%7B%7D&highlight=0000ff&edit=_blank&layers=1&nav=1#R5VrLcpswFP0aL5MBBH4sazttM5O2mWTRdtVRQAa1gssI4Ue%2FvlIQ2CCcuB0b2rIyOghJnHNfkhmhRbx9x3EafYCAsJFjBdsRWo4cx7annvxRyK5Axh4qgJDTQHfaA4%2F0J9GgpdGcBiSrdRQATNC0DvqQJMQXNQxzDpt6txWw%2BqwpDokBPPqYmehnGoioQKfOZI%2B%2FJzSMypnt8ay4E%2BOys36TLMIBbA4gdDNCCw4giqt4uyBMkVfyUjz39sjdamGcJOKUB35%2Bcx6u4mwF08ld8PF%2BnUdbdKVHWWOW6xf%2BAGvCr9%2Fc3%2BpVi11JxSaigjym2FftjZR7hOaRiJls2fISZ2khwIpuiZxzrocmXJDt0TXbFRPShAjERPCd7KIfmGnr0cZjjzWXmwMpNBQdqFBiWIsfVgPv%2BZEXmqLfoQsZrJBA2otuAhcRhJBgdrNH5xzyJFCMLC3Z2ve5A0g1d9%2BJEDtt%2FDgXUGe2mFNN9DKNcl2Qc5%2B88AKudiHMQyJe6Ddul4UThgVd19dxfpanA2HZdvuk2T3i%2B0ss8N%2Fg%2FGOn4f2Tnr1%2FfISwBXBiWixjMkcpy%2ByBOplma9RNTeaccQt1FXh27iYGd5%2Bx8KM%2Fp%2B0MLCFnVmPJcU2abKdLC5sZLN0ma%2Fk2oMfujSnUcEXL65kp2%2BkjR0i6%2BO6LbFxZ15bkQCNf1YjXrjcrgeVWz1G0doete8Kp5IBwDZ4x75QF8t%2Bd3c2880ACmhl6yoI5VZf%2BjlEpG0evm%2FpTIfDdUwVg%2F0f4LPunXMhhiMazQmHbO1OqaviHMzMjSVu8vZx7eC25KglhOf%2BXaZ64DZq9nmmuPLOfKGRd10NQz%2BHn5LLX6zP%2BOGYZt4QY08SQUhqqqDOfCQ4%2FyAIYKPoSUIY%2BX1HGGhBmNEyUT0kOFdNzZfbUx%2ByNvhHTIGDHcnfdQM6Rvr16oYNOcxt0Mbcxq8HWTcd%2FI0AzP7TVT90qMDUUuE1WHEt%2Bc1%2FkLfuZ%2F0aLivtSi769ATmGFguQoRoYI9wsi0whTieYE5mR8dPzUIrOFGgint%2FHm4%2B8pRpLZpsya%2F%2BGxmfQZfZ6bm%2Fb7V9OFtQiSxzniWRWUEhU1tz6JFXXw1GpVKVUqeVkoTqn6UYmcztxB2FIk3AwmthWXRRkm6J0q4m5%2B3hHEll4%2BgVIA%2B1BAxFoUg9t6NRzpssJZNbA8zyT%2B7ZMRjKLQaiUGog4qLGpRC2Jp%2FKwbtQxy%2BNHwtfUJ8PJM2jciGktJyrdxjSzYl7kmYB4mHUAKrfRWh%2B33FEceg3qVCDz8Lzc11ux%2Bi5hQOK4dedxW%2F45s%2B0uxSkXdHgeidN0SFVaM6J5fVdprvnxxwNJJTsCOB1Qqmn%2Bz9yWamadCmMeCCyf1JGAFGC4srjOxWSRzf13WM%2F3Dr5mQze%2FAA%3D%3D)

## Watch API
An API that calculate least angle of watch hands

### Features

- Calculate least angle of watch hands providing to,e

### Getting Started

Follow these steps to set up the Watch API on your local machine.

### Prerequisites

- .NET SDK
Application already uses Redis connection in cloud in order to save responses.

### Installation

1. Clone the repository
2. Run solution with Mover.API as startup project
3. Navigate in swagger to the WatchHands controller
4. Try calling endpoints using Swagger!

### API Endpoints
GET /api/watchhands/item: Calculate least angle between watch hands by given DataTime parameter

For detailed API documentation, refer to the Swagger UI.




## Inventory Management System

An inventory management system for tracking and managing inventory items.

### Features

- Create and update inventory items
- Remove specified quantities from inventory
- Retrieve details of inventory items
- View a list of all inventory items

### Getting Started

Follow these steps to set up the Inventory Management System on your local machine.

### Prerequisites

- .NET SDK
Application already uses MongoDB connection to database in cloud.

### Installation

1. Clone the repository
2. Run solution with Mover.API as startup project
3. Navigate in swagger to the Inventory controller
4. Try calling endpoints using Swagger!

   `
### API Endpoints
POST /api/inventory/item: Create or update an inventory item.

PUT /api/inventory/item/remove-quantity: Remove a specified quantity from an inventory item.

GET /api/inventory/item/{sku}: Retrieve details of an inventory item by SKU.

GET /api/inventory/item: Retrieve details of all inventory items.

For detailed API documentation, refer to the Swagger UI.



## Possible additions:

### API:
- Authentication
- Connect Redis sink to logging feature
- Extend generic validation

