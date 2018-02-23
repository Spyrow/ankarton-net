# Ankarton-NET

This is a tool to create accounts for Ankama in bulk.

## Installation

1. Download the project from:

- [Direct Link](https://github.com/yovanoc/ankarton-net/archive/master.zip)

- [Github](https://github.com/yovanoc/ankarton-net)

2. Install **.Net Core SDK** from:

- [Microsoft Downloads](https://www.microsoft.com/net/download/)

3. Extract the zip file to a location in our case: `c:\`

4. Navigate to the location through a terminal: `cd C:\ankarton-net-master\app`

5. Run the command: `dotnet run 10 accounts.txt`

**PS: Accounts will be generated in Documents by default.**

---

## Command Structure

Here are the possible parameters for running **Ankarton**.

`ACCOUNT_NUMBER` = The number of accounts to be generated.

`PROXY_LIST` = Path to a proxy list if you have one. **Optional parameter**.

`ACCOUNT_LIST` = Name of the file the generated accounts should be saved to.

**PS: The `ACCOUNT_LIST` file is generated in Documents.**

## Examples

1. `dotnet run 10 accounts.txt`

This will generate 10 accounts and write them in `accounts.txt`

___

2. `dotnet run 7 my_proxies.txt my_accounts.txt`

This will generate 7 accounts using a the proxies from the file `my_proxies.txt` and save them in the file `my_accounts.txt`

---

## NPM Version

There is an NPM version existent:

[Ankarton NPM Verion](https://www.npmjs.com/package/ankarton)

From different tests done the **NPM Version** seems to be more stable.

### NPM Install

Simply run

`npm install -g ankarton`

### NPM Usage

To execute run

`ankarton --out ACCOUNTS.txt --proxy PROXY-LIST.txt --total ACCOUNT_NUMBER`

**PS: Using a proxy list is optional but will be faster.**
