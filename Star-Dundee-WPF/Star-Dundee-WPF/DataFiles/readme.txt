Industrial Team Project Example Files
-------------------------------------

These files demonstrate the following errors:

Test 1: This shows an invalid sequence number.

Test 2: This shows a packet timing out at a router.  The link 1 recording shows
        the traffic before it enters the router, with link 3 showing the traffic
        once it has been routed.

Test 3: This shows a babbling idiot which repeatedly sends the same packet until
        it is detected.

Test 4: This shows an invalid CRC in an RMAP packet.  Link 1 shows the RMAP
        commands and link 2 shows the replies.  To detect the cause of this
        error you would need to know the format of RMAP packets and check the
		CRCs of each one.

Test 5: This shows a packet which is longer than expected.  Again this is an
        RMAP packet with link 1 showing the commands and link 2 the replies.  As
        the packets are RMAP packets, the error could be detected by checking
        the CRC or determining the expected length from the RMAP fields.
        Alternatively the error could be found by looking at the previous
        packets.  

Note that after each error is encountered, the link is disconnected.
