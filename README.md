# AuctionHouse

Gennemgangen er linuxbaseret og benytter WSL som docker backend

- Start med at åbne WSL i roden af repositoriet.
- Sikre init-secret scripts ikke er windowsbaseret med hidden charaters
  - 'dos2unix init-secrets.sh'
- herefter kører man blot docker-compose up
- åben følgende browser tabs:
  - vault
    - http://localhost:8201
  - auth
    - http://localhost:5051
  - catalog
    - http://localhost:6051
  - auction
    - http://localhost:7051
  - bid
    - http://localhost:8051

Vault er blot til at se alle de nødvendige secrets som skal bruges af de forskellige services

1. Register en bruger i auth-servicen med '[POST]/api/account/register' - udfyld body
2. Login med din nye bruger i auth-servicen med '[POST]/api/account/login' - udfyld username + password
3. Kopier token (dette skal bruges i de andre services)
4. Tryk på Authorize knappen i swagger --> indsæt herefter 'Bearer + [kopieret token]' og tryk login.
5. Valider at auithorization virker ved at benytte current user endpointet: '[GET]/api/account/currentUser'
6. Responsen skal indeholde email + token for at token er gyldig.
7. Naviger til Catalog service swagger
8. Tryk på Authorize knappen --> indsæt herefter 'Bearer + [kopieret token]' og tryk login.
9. Opret en vare i kataloget i catalog-servicen med '[POST]/Catalog/CreateProduct' - udfyld body (productId fjernes, da dette oprettes af databasen + isSold sættes til false).
10. Kopier herefter productId fra responsen
11. Valider at produktet er oprette ved at bruge endpointet: '[GET]/Catalog/[ProductId]' - indsæt productId i path
12. Naviger til Auction service swagger
13. Tryk på Authorize knappen --> indsæt herefter 'Bearer + [kopieret token]' og tryk login.
14. Opret en auction ved at benytte endpointet: '[POST]/api/Auction' - udfyld body (Herunder productId, samt tidpunkt for planlagt auction).
15. Kopier auctionId fra responsen
16. Valider at auktionen er oprette ved at bruge endpointet: '[GET]/api/Auction/[auctionId]' - indsæt auctionId i path.
17. For at man kan bryde for en auction, skal den have status 1 (0 = oprettet, 1 = startet, 2 = afsluttet).
18. Dette simulerer vi ved at benytte mock endpointet: '[PUT]/api/Auction/start/[auctionId]' - indsæt auctionId i path.
19. Valider gerne at auktionen har fået en ny status ved at bruge endpointet: '[GET]/api/Auction/[auctionId]' - indsæt auctionId i path.
20. Naviger herefter til Bid service swagger
21. Tryk på Authorize knappen --> indsæt herefter 'Bearer + [kopieret token]' og tryk login.
22. Placer hermed et bud på auktionen ved at kalde endpoinet: '[POST]/api/Bid' - udfyldt body med auctionId, bud som er over mindstepris (askingPrice), samt userId (skriv blot 0 - dette userId kan dog trækkes ud af jwt token propertien: nameid) (benyt evt. https://jwt.io til at decode token for at få det korrekte userId).
23. Naviger herefter tilbage til Auction service swagger
24. Valider budet ved at benytte endpointet: '[GET]/api/Auction/[auctionId]' - indsæt auctionId i path.
25. bidSummary burde herefter være udfyldt med: userId, nyt bud samt inkrementering i totalBids.
26. Afslut auktione før tid, ved at benytte mock endpointet: '[PUT]/api/Auction/end/[auctionId]' - indsæt auctionId i path.
27. Valider herefter at auktionen er afsluttet ved at benytte endpointet: '[GET]/api/Auction/[auctionId]' - indsæt auctionId i path.
