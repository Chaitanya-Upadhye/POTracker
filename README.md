# PO Tracker

Web application for automating purchase order tracking process.

This application will:
1. Poll your mail server every hour to process incoming emails.
2. Parse pdf attachment of a mail received from a specified receiver, based on template provided for a specific vendors PO pdf format.
3. Persists the data and notifies the client on latest Purchase Orders recieved on WhatsApp via Twilio.
4. ReactJS client application will allow client to edit PO data and add supplier information.
