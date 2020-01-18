# Contracts

## Marble Placement
| bytes | 1       | 1    | 8      | ... | 1    | 8      |
|-------|---------|------|--------|-----|------|--------|
| data  | # types | type | amount | ... | type | amount |

## Start Game Contract
| bytes | 1       | 1             | ?               | ?            | ?            | 20             | 20             | 8     | 64           | 64           |
|-------|---------|---------------|-----------------|--------------|--------------|----------------|----------------|-------|--------------|--------------|
| data  | version | contract type | fee (placement) | J1 placement | J2 placement | J1 PubKey Hash | J2 PubKey Hash | Nonce | J1 signature | J2 signature |
Contract type: 0

## Throw Contract
| bytes | 1       | 1             | ?               | 1 | 1 | 32        | 8     | 64        |
|-------|---------|---------------|-----------------|---|---|-----------|-------|-----------|
| data  | version | contract type | fee (placement) | X | Z | game hash | nonce | signature |
Contract type 1

## Transaction Contract
| bytes | 1       | 1             | ?               | ?            | ?            | 20             | 20             | 8     | 64           | 64           |
|-------|---------|---------------|-----------------|--------------|--------------|----------------|----------------|-------|--------------|--------------|
| data  | version | contract type | fee (placement) | J1 placement | J2 placement | J1 PubKey Hash | J2 PubKey Hash | Nonce | J1 signature | J2 signature |
Contract type: 2