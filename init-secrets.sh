#!/bin/sh

export VAULT_ADDR="https://vault:8201"
export VAULT_TOKEN="00000000-0000-0000-0000-000000000000"
export SECRETS_FILE="/secrets.json"
export VAULT_CACERT="/data/certs/cert.pem" # Path to the CA certificate

echo "Vault Started."

echo "Authenticate into Vault"
# Authenticate to Vault
vault login $VAULT_TOKEN

vault secrets enable -path=secret kv

vault kv put secret/services @"$SECRETS_FILE"