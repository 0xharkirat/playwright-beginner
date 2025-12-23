import { NextResponse } from 'next/server';

export async function GET() {
    try {
        // Server-side: use Docker service name or localhost
        const serviceOneUrl = process.env.SERVICE_ONE_URL || 'http://localhost:5001';
        const response = await fetch(`${serviceOneUrl}/api/Hello`);

        if (!response.ok) {
            return NextResponse.json(
                { error: `Service-One returned ${response.status}` },
                { status: response.status }
            );
        }

        const data = await response.text();
        return NextResponse.json({ message: data });
    } catch (error) {
        console.error('Error fetching from Service-One:', error);
        return NextResponse.json(
            { error: 'Failed to fetch from Service-One' },
            { status: 500 }
        );
    }
}
