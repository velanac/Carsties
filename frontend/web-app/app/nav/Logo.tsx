'use client';

import { useParamsStore } from '@/hooks/useParamsStore';
import { usePathname, useRouter } from 'next/navigation';
import { AiOutlineCar } from 'react-icons/ai';

export default function Logo() {
  const router = useRouter();
  const pathname = usePathname();
  const reset = useParamsStore((state) => state.reset);

  return (
    <div
      onClick={() => {
        if (pathname !== '/') {
          router.push('/');
        } else {
          reset();
        }
      }}
      className='cursor-pointer flex items-center gap-2 text-3xl font-semibold text-red-500'
    >
      <AiOutlineCar size={32} />
      <div>Carsties Auctions</div>
    </div>
  );
}
