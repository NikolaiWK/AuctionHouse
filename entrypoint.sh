#!/bin/sh

set -e

# Start Vault server in the background
vault server -dev -dev-root-token-id="00000000-0000-0000-0000-000000000000" &

# Wait for Vault to be ready
sleep 5

# Run the initialization script
if [ -f /init-secrets.sh ]; then
  sh /init-secrets.sh
else
  echo "Initialization script not found. Skipping..."
fi

# Bring the Vault process to the foreground
wait